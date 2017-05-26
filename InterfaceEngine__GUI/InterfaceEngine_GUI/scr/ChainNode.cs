using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEngine_GUI
{
    class ChainNode
    {
        private List<ChainNode> _dependants = new List<ChainNode>();
        private List<ChainNode> _dependingOn = new List<ChainNode>();
        private Statement _stat;

        public Statement Stat
        {
            get
            {
                return _stat;
            }
        }

        public ChainNode(Statement stat)
        {
            _stat = stat;
        }

        public void AddDependants(ChainNode toAdd)
        {
            bool found = false;

            foreach (ChainNode c in _dependants)
            {
                if (toAdd.Stat.Identifier == c.Stat.Identifier)
                {
                    found = true;

                    break;
                }
            }

            if (!found)
            {
                _dependants.Add(toAdd);
            }
        }

        public void AddDependancies(ChainNode toAdd)
        {
            bool found = false;

            foreach (ChainNode c in _dependingOn)
            {
                if (toAdd.Stat.Identifier == c.Stat.Identifier)
                {
                    found = true;

                    break;
                }
            }

            if (!found)
            {
                _dependingOn.Add(toAdd);
            }
        }

    }
}
