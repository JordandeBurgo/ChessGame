using Microsoft.VisualStudio.TestTools.UnitTesting;
using CollegeProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace CollegeProject.Tests
{
    [TestClass()]
    public class Form1Tests
    {
        [TestMethod()]
        public void Form1Test()
        {
            Form1 form1 = new Form1("M1B");
            if (!form1.timed)
            {
                Assert.Fail();
            }
            form1 = new Form1("M2B");
            if (!form1.timed)
            {
                Assert.Fail();
            }
            form1 = new Form1("M3S");
            if (!form1.suicide)
            {
                Assert.Fail();
            }
            form1 = new Form1("S1S");
            if (form1.timed || form1.suicide)
            {
                Assert.Fail();
            }
        }

        [TestMethod()]
        public void Form1Test2()
        {
            Form1 form = new Form1("M1B");
            foreach(Panel p in form.generation.getPanels())
            {
                if(!(p.BackColor ==
                     ColorTranslator.FromHtml("#80A83E") ||
                     p.BackColor ==
                     ColorTranslator.FromHtml("#D9DD76")))
                {
                    Assert.Fail();
                }
            }
        }
    }
}