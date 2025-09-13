using System.Text;

namespace HeepWare.RTree.Library
{
    public class Connection
    {
        public string meshName = string.Empty;
        private List<string> connected;
        private bool processed = false;
        private int countForwardConnections = -1;

        public Connection()
        {
            connected = new List<string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetForwardConnections()
        {
            return countForwardConnections;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool GetProcessed()
        { return processed; }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool SetProcessed()
        {
            processed = true;
            return processed;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetConnections()
        {
            return connected;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int ConnectionCount()
        { return connected.Count; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="names"></param>
        public void AddRange(List<string> names)
        {
            if (names != null && names.Count > 0)
            {
                for (int i = 0; i < names.Count; i++)
                {
                    if (names[i] != meshName)
                    {
                        connected.Add(names[i]);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public void Add(string name)
        {
            if (name != null)
            {
                if (name != meshName)
                {
                    connected.Add(name);
                }
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("==> Connection mesh name {0} processed {1}\n", meshName, processed);
            for (int i = 0; i < connected.Count; i++)
            {
                sb.AppendFormat("\t --> {0}\n", connected[i]);
            }
            sb.AppendLine("->End Connection");
            return sb.ToString();
        }
    }
}
