using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Version_1._0.ViewModel
{
    public class ViewModel
    {

        private readonly Model.Work _work = new Model.Work();
        public ViewModel() 
        {
            //input = "";
            //output = "";
        }
        public string input { get; set; }
        public string output { get; set; }

        public string GetName()
        {
            return output = _work.name;
        }

        public void SetName(string input)
        {
            _work.name = input;
        }

        public string GetSource()
        {
            return output = _work.source;
        }
        public void SetSource(string input)
        {
            _work.source = input;
        }
        public string GetTarget()
        {
            return output = _work.target;
        }
        public void SetTarget(string input)
        {
            _work.target = input;
        }

        public string GetType()
        {
            return output = _work.type;
        }
        public void SetType(string input)
        {
            _work.type = input;
        }




    }
}
