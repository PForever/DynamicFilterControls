using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DynamicFilterControls
{
    [Docking(DockingBehavior.Ask)]
    [Designer(typeof(StylishedDataGridViewControlDesigner))]
    public partial class StylishedDataGridView : UserControl
    {
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DataGridView Grid => dataGridView1;
        public StylishedDataGridView()
        {
            InitializeComponent();
        }
    }
    public class StylishedDataGridViewControlDesigner : System.Windows.Forms.Design.ControlDesigner
    {
        public override void Initialize(IComponent comp)
        {
            base.Initialize(comp);
            var uc = (StylishedDataGridView)comp;
            EnableDesignMode(uc.Grid, "Grid");
        }
    }
}
