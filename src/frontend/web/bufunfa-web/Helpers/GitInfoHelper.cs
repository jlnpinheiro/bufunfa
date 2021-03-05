using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JNogueira.Bufunfa.Web.Helpers
{
    /// <summary>
    /// Classe para obter as informações de commit do Git
    /// </summary>
    public class GitInfoHelper
    {
        public string Branch { get; }

        public string Versao { get; }

        public string Commit { get; }

        public string ResponsavelCommit { get; }

        public string DataComit { get; }

        public GitInfoHelper()
        {
            IEnumerable<string> gitInfo;

            using (var stream = typeof(GitInfoHelper).Assembly.GetManifestResourceStream("JNogueira.Bufunfa.Web.git-info.txt"))
            using (var reader = new StreamReader(stream))
            {
                gitInfo = reader.ReadToEnd().Split('\n');
            }

            this.Branch            = gitInfo.ElementAt(0);
            this.Versao            = gitInfo.ElementAt(1).Substring(0, 7);
            this.Commit            = gitInfo.ElementAt(1);
            this.ResponsavelCommit = gitInfo.ElementAt(2)?.ToUpper();
            this.DataComit         = gitInfo.ElementAt(3);
        }
    }
}
