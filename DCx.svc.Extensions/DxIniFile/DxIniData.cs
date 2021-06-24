using DCx.List;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DCx.IniData
{
    public class DxIniData : IDxIniData
    {
        #region Properties

        public  List<string>                IniComments     { get; private set; }   = new List<string>();
        public  DictTxtOf<IDxIniSection>    IniSections     { get; private set; }   = new DictTxtOf<IDxIniSection>();

        #endregion


        #region ParseString()

        public void ParseString (bool splitValues, string iniValue)
        {
            if (iniValue.IsUsed())
            {
                string[]        iniLines    = iniValue.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                IDxIniSection   curSection  = null;
                
                for (int i=0; i<iniLines?.Length; i++)
                {
                    DxIniEntry.ParseLine(splitValues, iniLines[i].Trim(),
                                        (secName)   => curSection = this.IniSections.MapValue(secName, new DxIniSection(splitValues, secName, false)),
                                        (iniEntry)  => curSection?.IniEntries.MapValue(iniEntry.EntryName, iniEntry));
                }
            }
        }
        #endregion

        
        #region AddComment()

        public void AddComment (string comment)  =>  this.IniComments.Add(comment);

        #endregion


        #region AddSection()

        public IDxIniSection AddSection (string sectionName, bool isCompressed) 
            =>  this.IniSections.MapValue(sectionName, new DxIniSection(false, sectionName, isCompressed));

        public IDxIniSection AddSection (bool splitValues, string sectionName, bool isCompressed) 
            =>  this.IniSections.MapValue(sectionName, new DxIniSection(splitValues, sectionName, isCompressed));

        #endregion


        #region GetString()

        public string GetString()
        {
            var sb = new StringBuilder();

            if (this.IniComments.Count > 0)
            {
                var commentLine = new String('*', 60);

                for (int i=0; i<this.IniComments.Count; i++)
                {
                    var comment = this.IniComments[i];
                    if (comment.Length < 2)
                    {
                        sb.AppendLine(commentLine);
                    }
                    else
                    {
                        sb.AppendLine($"* {comment}");
                    }
                }
                sb.Append(Environment.NewLine);
            }

            if (this.IniSections.Count > 0)
            {
                foreach(IDxIniSection iniSection in this.IniSections.ValueList)
                {
                    sb.AppendLine(iniSection.GetString());
                }
            }
            return sb.ToString();
        }
        #endregion
    }
}
