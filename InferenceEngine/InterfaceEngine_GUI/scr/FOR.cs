using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceEngine_GUI
{
    class FOR
    {
        private String _name;
        private int _size1;
        private int _size2;

        public FOR()
        {

        }

        public String Name
        {
            get { return _name; }

            set {_name = value;}
        }

        public void SetSize(int size1, int size2)
        {
            _size1 = size1;
            _size2 = size2;
        }

        public int Size1
        {
            get { return _size1; }

            set { _size1 = value; }
        }

        public int Size2
        {
            get { return _size2; }

            set { _size2 = value; }
        }


    }
}
