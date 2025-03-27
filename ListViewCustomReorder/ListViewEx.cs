using System;
using System.Drawing;
using System.Windows.Forms;

namespace ListViewCustomReorder
{
	// Token: 0x02000003 RID: 3
	public class ListViewEx : ListView
	{
		// Token: 0x06000002 RID: 2 RVA: 0x0000219A File Offset: 0x0000039A
		public ListViewEx()
		{
			base.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000021C0 File Offset: 0x000003C0
		// (set) Token: 0x06000004 RID: 4 RVA: 0x000021D8 File Offset: 0x000003D8
		public int LineBefore
		{
			get
			{
				return this._LineBefore;
			}
			set
			{
				this._LineBefore = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000005 RID: 5 RVA: 0x000021E4 File Offset: 0x000003E4
		// (set) Token: 0x06000006 RID: 6 RVA: 0x000021FC File Offset: 0x000003FC
		public int LineAfter
		{
			get
			{
				return this._LineAfter;
			}
			set
			{
				this._LineAfter = value;
			}
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002208 File Offset: 0x00000408
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);
			bool flag = m.Msg == 15;
			if (flag)
			{
				bool flag2 = this.LineBefore >= 0 && this.LineBefore < base.Items.Count;
				if (flag2)
				{
					Rectangle rc = base.Items[this.LineBefore].GetBounds(ItemBoundsPortion.Entire);
					this.DrawInsertionLine(rc.Left, rc.Right, rc.Top);
				}
				bool flag3 = this.LineAfter >= 0 && this.LineBefore < base.Items.Count;
				if (flag3)
				{
					Rectangle rc2 = base.Items[this.LineAfter].GetBounds(ItemBoundsPortion.Entire);
					this.DrawInsertionLine(rc2.Left, rc2.Right, rc2.Bottom);
				}
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000022E4 File Offset: 0x000004E4
		private void DrawInsertionLine(int X1, int X2, int Y)
		{
			using (Graphics g = base.CreateGraphics())
			{
				g.DrawLine(Pens.Orange, X1, Y, X2 - 1, Y);
				Point[] leftTriangle = new Point[]
				{
					new Point(X1, Y - 4),
					new Point(X1 + 7, Y),
					new Point(X1, Y + 4)
				};
				Point[] rightTriangle = new Point[]
				{
					new Point(X2, Y - 4),
					new Point(X2 - 8, Y),
					new Point(X2, Y + 4)
				};
				g.FillPolygon(Brushes.Orange, leftTriangle);
				g.FillPolygon(Brushes.Orange, rightTriangle);
			}
		}

		// Token: 0x04000001 RID: 1
		private const int WM_PAINT = 15;

		// Token: 0x04000002 RID: 2
		private int _LineBefore = -1;

		// Token: 0x04000003 RID: 3
		private int _LineAfter = -1;
	}
}
