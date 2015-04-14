using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TSF;

//References
//http://www.kanazawa-net.ne.jp/~pmansato/net/net_tech_ime.htm
//https://github.com/NyaRuRu/TSF-SearchCandidateProvider
//http://handcraft.blogsite.org/Memo/Article/Archives/424
//http://www.atmarkit.co.jp/fdotnet/dotnettips/875imeyomi/imeyomi.html
//http://tu3.jp/0964

namespace ConsoleApplication1
{
    public class ImeLanguage
    {
        private IFELanguage ifelang;

        public ImeLanguage()
        {
            const int S_OK = 0;
            ifelang = null;
            try
            {
                ifelang = Activator.CreateInstance(Type.GetTypeFromProgID("MSIME.Japan")) as IFELanguage;

                int hr = ifelang.Open();
                if (hr != 0)
                    throw Marshal.GetExceptionForHR(hr);


  

            }
            catch (COMException ex)
            {
                if (ifelang != null) ifelang.Close();
            }
        }

        public static string[] GetIMEKanaToKanjiCandidates(string jpstr)
        {
            using (var provider = SearchCandidateProvider.FromCurrentProfile().Result)
            {
                if (provider == null)
                {
                    Console.WriteLine("No provider found.");
                    return null;
                }
                Debug.WriteLine("{0} is found", provider.Profile.Name);

                if (string.IsNullOrEmpty(jpstr))
                {
                    return new string[1];
                }

                var result = provider.GetSearchCandidates(jpstr).Result;
                Debug.WriteLine("Elapsed time: {0} msec", result.Elaplsed.TotalMilliseconds);
                return result.Candidates.ToArray();
            }
        }

        public string GetIMEKanjiToKana(string jpstr)
        {
            string yomigana = "";
            int hr = ifelang.GetPhonetic(jpstr, 1, -1, out yomigana);
            if (hr != 0)
                throw Marshal.GetExceptionForHR(hr);

            Debug.WriteLine(yomigana);
            return yomigana;
        }

        public void Close()
        {
            if (ifelang != null) ifelang.Close();
        }
    }

    // IFELanguage2 Interface ID
    //[Guid("21164102-C24A-11d1-851A-00C04FCC6B14")]
    [ComImport]
    [Guid("019F7152-E6DB-11D0-83C3-00C04FDDB82E")]
    [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IFELanguage
    {
        int Open();
        int Close();
        int GetJMorphResult(uint dwRequest, uint dwCMode, int cwchInput, [MarshalAs(UnmanagedType.LPWStr)] string pwchInput, IntPtr pfCInfo, out object ppResult);
        int GetConversionModeCaps(ref uint pdwCaps);
        int GetPhonetic([MarshalAs(UnmanagedType.BStr)] string @string, int start, int length, [MarshalAs(UnmanagedType.BStr)] out string result);
        int GetConversion([MarshalAs(UnmanagedType.BStr)] string @string, int start, int length, [MarshalAs(UnmanagedType.BStr)] out string result);
    }

}