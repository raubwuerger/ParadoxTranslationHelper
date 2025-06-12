using Serilog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParadoxTranslationHelper.Comparator
{
    public class ComparatorNestedString
    {
        List<string> _onlyInOrg;
        public List<string> OnlyInOrg
        {
            get { return _onlyInOrg; }
        }
        List<string> _onlyInToVerify;
        public List<string> OnlyInToVerify
        {
            get { return _onlyInToVerify; }
        }
        public void Compare(List<string> org, List<string> toVerify)
        {
            _onlyInOrg = null;
            _onlyInToVerify = null;

            if (org == null)
            {
                Log.Verbose("Parameter <List<string>::org> must not be null!");
                return;
            }

            if (toVerify == null)
            {
                Log.Verbose("Parameter <List<string>::toVerify> must not be null!");
                return;
            }

            if (org.Count() == 0 && toVerify.Count() == 0)
            {
                _onlyInOrg = org;
                _onlyInToVerify = toVerify;
                return;
            }

            if (toVerify.Count() == 0)
            {
                Log.Verbose("Parameter <List<string>::toVerify> must not be empty!");
                return;
            }

            if (org.Count() == 0)
            {
                Log.Verbose("Parameter <List<string>::org> must not be empty!");
                return;
            }

            _onlyInToVerify = toVerify.Except(org).ToList<string>();
            _onlyInOrg = org.Except(toVerify).ToList<string>();

            return;
        }

        public bool Ok()
        {
            if( null == OnlyInOrg )
            {
                return false;
            }

            if (null == _onlyInToVerify )
            {
                return false;
            }

            if (_onlyInOrg.Count != _onlyInToVerify.Count )
            {
                return false;
            }

            if (_onlyInOrg.Count() == 0 && _onlyInToVerify.Count() == 0)
            {
                return true;
            }

            //TODO: 2025-06-12 - JHA - Is this realy neccessary?
            return Enumerable.SequenceEqual(_onlyInOrg, _onlyInToVerify);
        }
    }
}
