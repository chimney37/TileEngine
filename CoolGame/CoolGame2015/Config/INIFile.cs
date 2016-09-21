using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;


namespace BasicTileEngineMono
{
    public class IniFile
    {
        #region DLL IMPORTS
        [DllImport("KERNEL32.DLL", 
            EntryPoint = "GetPrivateProfileStringW",
            SetLastError = true,
            CharSet = CharSet.Unicode, 
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int GetPrivateProfileString(
          string lpSection,
          string lpKey,
          string lpDefault,
          StringBuilder lpReturnString,
          int nSize,
          string lpFileName);

        [DllImport("KERNEL32.DLL", 
            EntryPoint = "WritePrivateProfileStringW",
            SetLastError = true,
            CharSet = CharSet.Unicode,
            ExactSpelling = true,
            CallingConvention = CallingConvention.StdCall)]
        private static extern int WritePrivateProfileString(
          string lpSection,
          string lpKey,
          string lpValue,
          string lpFileName);
        #endregion

        #region PROPERTIES
        private string _path = "";
        public string Path
        {
            get { return _path; }
            set
            {
                if (!File.Exists(value))
                    File.WriteAllText(value, "", Encoding.Unicode);
                
                _path = value;
            }
        }
        #endregion

        #region CONSTRUCTOR
        /// <summary>
        /// INIFile Constructor.
        /// </summary>
        /// <PARAM name="INIPath"></PARAM>
        public IniFile(string INIPath)
        {
            this.Path = INIPath;
        }
        #endregion

        #region MEMBERS
        /// <summary>
        /// Write Data to the INI File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// Section name
        /// <PARAM name="Key"></PARAM>
        /// Key Name
        /// <PARAM name="Value"></PARAM>
        /// Value Name
        public void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, this.Path);
        }

        /// <summary>
        /// Read Data Value From the Ini File
        /// </summary>
        /// <PARAM name="Section"></PARAM>
        /// <PARAM name="Key"></PARAM>
        /// <PARAM name="Path"></PARAM>
        /// <returns></returns>
        public string[] IniReadValue(string Section, string Key)
        {
            const int MAX_CHARS = 1023;
            StringBuilder result = new StringBuilder(MAX_CHARS);
            GetPrivateProfileString(Section, Key, "", result, MAX_CHARS, this.Path);
            return result.ToString().Split('\0');
        }

        /// <summary>
        /// General ini reader
        /// Reference:
        /// http://smdn.jp/programming/netfx/tips/read_ini/
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, Dictionary<string, string>> ReadIni()
        {
            using (var reader = new StreamReader(this.Path))
            {
                var sections = new Dictionary<string, Dictionary<string, string>>(StringComparer.Ordinal);
                var regexSection = new Regex(@"^\s*\[(?<section>[^\]]+)\].*$", RegexOptions.Singleline | RegexOptions.CultureInvariant);
                var regexNameValue = new Regex(@"^\s*(?<name>[^=]+)=(?<value>.*?)(\s+;(?<comment>.*))?$", RegexOptions.Singleline | RegexOptions.CultureInvariant);
                var currentSection = string.Empty;

                // セクション名が明示されていない先頭部分のセクション名を""として扱う
                sections[string.Empty] = new Dictionary<string, string>();

                for (; ; )
                {
                    var line = reader.ReadLine();

                    if (line == null)
                        break;

                    // 空行は読み飛ばす
                    if (line.Length == 0)
                        continue;

                    // コメント行は読み飛ばす
                    if (line.StartsWith(";", StringComparison.Ordinal))
                        continue;
                    else if (line.StartsWith("#", StringComparison.Ordinal))
                        continue;

                    var matchNameValue = regexNameValue.Match(line);

                    if (matchNameValue.Success)
                    {
                        // name=valueの行
                        // modification: if duplicate keys encountered, separate them by '|' character
                        if (!sections[currentSection].ContainsKey(matchNameValue.Groups["name"].Value.Trim()) )
                            sections[currentSection][matchNameValue.Groups["name"].Value.Trim()] = matchNameValue.Groups["value"].Value.Trim();
                        else
                            sections[currentSection][matchNameValue.Groups["name"].Value.Trim()] += "|" + matchNameValue.Groups["value"].Value.Trim();

                        continue;
                    }

                    var matchSection = regexSection.Match(line);

                    if (matchSection.Success)
                    {
                        // [section]の行
                        currentSection = matchSection.Groups["section"].Value;

                        if (!sections.ContainsKey(currentSection))
                            sections[currentSection] = new Dictionary<string, string>();

                        continue;
                    }
                }

                return sections;
            }
        }
    
        #endregion
    }
}