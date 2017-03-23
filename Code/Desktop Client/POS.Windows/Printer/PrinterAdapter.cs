using POSEntities.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
namespace POS.Windows.Printer
{
    public class PrinterAdapter
    {
        private static class PaymentType
        {
            public const string Cash = "מזומן";
            public const string CraditCard = "אשראי";

        }
        private static PrinterAdapter instance;
        public static PrinterAdapter Instance
        {
            get
            {
                instance = new PrinterAdapter();
                return instance;
            }
        }
        private PrintDocument pdoc = null;
        private Graphics graphics;
        private Font XLargFont;
        private Font largFont;
        private Font largFontBold;
        private Font mediumFont;
        private Font mediumBoldFont;
        private Font smallFont;
        private SolidBrush blackBrush;
        private int center;
        private double cashStatment;
        private int rightPage;
        private Point position;
        private StringFormat sformat;
        private Order order;
        private XReportHolder XorZReport;

        private StringFormat sfRTL = new StringFormat(StringFormatFlags.DirectionRightToLeft);
        private PrinterAdapter()
        {
            sformat = new StringFormat();
            XLargFont = new Font("Gisha", 20, FontStyle.Bold);
            largFont = new Font("Gisha", 16, FontStyle.Regular);
            largFontBold = new Font("Gisha", 16, FontStyle.Bold);
            mediumFont = new Font("Gisha", 10, FontStyle.Regular);
            mediumBoldFont = new Font("Gisha", 10, FontStyle.Bold);
            smallFont = new Font("Gisha", 8, FontStyle.Regular);
            blackBrush = new SolidBrush(Color.Black);
            center = 150;

            rightPage = 280;
            position = new Point();
            sformat.LineAlignment = StringAlignment.Center;
            sformat.Alignment = StringAlignment.Center;
            pdoc = new PrintDocument();
            PrinterSettings ps = new PrinterSettings();

            PaperSize psize = new PaperSize("Custom", 300, 200);
            ps.DefaultPageSettings.PaperSize = psize;
            pdoc.DefaultPageSettings.PaperSize = new PaperSize("Custom", 300, 1000);

        }
        public void printRecipte(Order ord)
        {

            order = ord;
            pdoc.PrintPage += new PrintPageEventHandler(pdoc_PrintOrderReceipt);
            openDialog();

        }
        void pdoc_PrintOrderReceipt(object sender, PrintPageEventArgs e)
        {

            graphics = e.Graphics;
            fillHeader();
            fillOrderNum();
            fillOrderSale();
            fillSummit();


        }
        private void fillOrderSale()
        {

            position.Y += smallFont.Height;
            position.X = 20;
            graphics.DrawString("כמות  |שם המוצר              |מחיר יח   |סהכ", mediumBoldFont, Brushes.Black, position);
            position.Y += mediumBoldFont.Height;
            //    graphics.DrawString("-----------------------------------------", mediumFont, Brushes.Black, position, sformat);
            position.X = center;
            graphics.DrawString("_________________________________________", mediumFont, Brushes.Black, position, sformat);
            OrderLine line;
            IEnumerator orderLinesEn = order.OrderLines.GetEnumerator();
            while (orderLinesEn.MoveNext())
            {
                line = orderLinesEn.Current as OrderLine;
                position.Y += mediumFont.Height;
                graphics.DrawString(line.quantity.ToString(), mediumFont, Brushes.Black, new PointF(270, position.Y), sfRTL);
                graphics.DrawString("|" + line.prodUnitPrice.ToString(), mediumFont, Brushes.Black, new PointF(120, position.Y), sfRTL);
                graphics.DrawString("|" + line.prodTotalPrice.ToString(), mediumFont, Brushes.Black, new PointF(55, position.Y), sfRTL);
                graphics.DrawString("|" + line.Product.prodName, mediumFont, Brushes.Black, new PointF(243, position.Y), sfRTL);
            } 
            position.Y += largFont.Height;
            position.X = center;
            graphics.DrawString("--------------------------------------------", mediumFont, Brushes.Black, position, sformat);
            position.Y += smallFont.Height;
            position.X = rightPage;
            printSumPlusTax(double.Parse(order.TotalPayment.ToString()));
            position.Y += mediumBoldFont.Height + 10;
            position.X = center;
            graphics.DrawString("--------------------------------------------", mediumFont, Brushes.Black, position, sformat);

            
            Payment payment;
            IEnumerator paymnetsEn = order.Payments.GetEnumerator();
            while (paymnetsEn.MoveNext())
            {
                position.Y += mediumBoldFont.Height;
                position.X = rightPage;
                payment = paymnetsEn.Current as Payment;
                graphics.DrawString("צורת תשלום: " + payment.PaymentTypeName, mediumFont, Brushes.Black, position, sfRTL);
                //  position.Y += mediumBoldFont.Height;
                //int payment = Int32.Parse(order.orderTable.Rows[0]["PaymentType"].ToString());
                switch (payment.PaymentTypeName)
                {
                    case PaymentType.Cash:

                        string recived = string.Format("₪{0:N2}", ((CashPayment)payment).RecievedAmount);
                        string change = string.Format("₪{0:N2}", ((CashPayment)payment).Change);
                        position.Y += mediumBoldFont.Height;
                        graphics.DrawString("התקבל: " + recived, mediumBoldFont, Brushes.Black, position, sfRTL);
                        position.Y += mediumBoldFont.Height;
                        graphics.DrawString("עודף: " + change, mediumBoldFont, Brushes.Black, position, sfRTL);
                        break;
                    case PaymentType.CraditCard:
                        break;
                    default:
                        break;
                }
            }
            position.Y += mediumBoldFont.Height;
            position.X = center;
            graphics.DrawString("--------------------------------------------", mediumFont, Brushes.Black, position, sformat);
            position.Y += mediumBoldFont.Height;
            position.X = rightPage;
            graphics.DrawString("שם העובד: " +order.Employee.FullName, mediumFont, Brushes.Black, position, sfRTL);
            position.Y += mediumBoldFont.Height;
            graphics.DrawString("שם הלקוח: " +order.Customer.FullName, mediumFont, Brushes.Black, position, sfRTL);
            position.Y += mediumBoldFont.Height;
            position.X = center;
            graphics.DrawString("--------------------------------------------", mediumFont, Brushes.Black, position, sformat);
            position.Y += mediumBoldFont.Height;
            position.X = rightPage;
            graphics.DrawString("תאריך: " +order.PurchaseDate.ToString(), mediumFont, Brushes.Black, position, sfRTL);
            position.Y += mediumBoldFont.Height;
            //graphics.DrawString("סניף: " + order.orderTable.Rows[0]["StoreID"].ToString(), mediumFont, Brushes.Black, position, sfRTL);
            //position.Y += mediumBoldFont.Height;
            position.X = center;


        }

        private void fillSummit()
        {
            position.Y += mediumBoldFont.Height;
            position.X = center;
            graphics.DrawString("--------------------------------------------", mediumBoldFont, Brushes.Black, position, sformat);
            position.Y += mediumBoldFont.Height;
            graphics.DrawString("תודה ולתראות! ", mediumFont, Brushes.Black, position, sfRTL);
            position.Y += mediumBoldFont.Height;
            position.X = center;
            graphics.DrawString("--------------------------------------------", mediumBoldFont, Brushes.Black, position, sformat);
        }

        private void fillOrderNum()
        {
            position.Y += smallFont.Height;
            position.X = rightPage;
            graphics.DrawString("מספר הזמנה יומית", smallFont, Brushes.Black, position, sfRTL);
            position.X = center;
            graphics.DrawString(order.DailyNumber.ToString().ToString(), mediumBoldFont, Brushes.Black, position);
            position.Y += mediumBoldFont.Height;
            position.X = center;
            graphics.DrawString("-----------------------------------------", mediumFont, Brushes.Black, position, sformat);
            position.X = rightPage;
            graphics.DrawString("חן מס/ קבלה" + "           " + order.ID.ToString(), largFontBold, Brushes.Black, position,sfRTL);
            position.Y += 35;
            position.X = center;
            graphics.DrawString("-----------------------------------------", mediumFont, Brushes.Black, position, sformat);
            position.Y += 10;
            position.X = center;
            graphics.DrawString("-----------------------------------------", mediumFont, Brushes.Black, position, sformat);
        }

        private void fillHeader()
        {
            position = new Point(center, 50);
            graphics.DrawString("מרכז הפיתה", XLargFont, Brushes.Black, position, sformat);
            position.Y += mediumFont.Height;
            graphics.DrawString("_________________________________________", mediumFont, Brushes.Black, position, sformat);
            position.Y += mediumFont.Height;
            graphics.DrawString("בן גוריון 1 ", mediumFont, Brushes.Black, position, sformat);
            position.Y += mediumFont.Height;
            graphics.DrawString("דימונה", mediumFont, Brushes.Black, position, sformat);
            position.Y += mediumFont.Height;
            graphics.DrawString("077-6898991", mediumFont, Brushes.Black, position, sformat);
            position.Y += mediumFont.Height;
            graphics.DrawString("ע.מ 12345678", mediumFont, Brushes.Black, position, sformat);
            position.Y += smallFont.Height;
            graphics.DrawString("-----------------------------------------", mediumFont, Brushes.Black, position, sformat);
        }

        public void PrintXreport(XReportHolder report)
        {
            XorZReport = report;
            pdoc.PrintPage += new PrintPageEventHandler(pdoc_PrintXReport);
            openDialog();

        }

        private void openDialog()
        {
            PrintPreviewDialog pp = new PrintPreviewDialog();
            pp.Document = pdoc;

            if (pp.ShowDialog() == DialogResult.OK)
            {
                pdoc.Print();
            }

        }

        private void pdoc_PrintXReport(object sender, PrintPageEventArgs e)
        {
            graphics = e.Graphics;
            fillHeader();
            fillXSetHeader();
            printPaymentType();
            fillStatistic();
            fillSummit();

        }

        private void fillXSetHeader()
        {
            position.X = center;
            position.Y += mediumBoldFont.Height;
            graphics.DrawString("X  " + "דוח", XLargFont, Brushes.Black, position, sformat);
            position.Y += mediumBoldFont.Height;
            position.X = rightPage;
            graphics.DrawString("עבור:", mediumFont, Brushes.Black, position, sfRTL);
            position.X -= 40;
            graphics.DrawString("קופה נוכחית", mediumBoldFont, Brushes.Black, position, sfRTL);
            position.Y += mediumBoldFont.Height;
            position.X = rightPage;
            //graphics.DrawString("עובד:   " + XorZReport.Employee.FullName, mediumBoldFont, Brushes.Black, position, sfRTL);
            //position.Y += mediumBoldFont.Height;
            graphics.DrawString("תאריך:   " + DateTime.Now.ToString(), mediumFont, Brushes.Black, position, sfRTL);
            position.Y += smallFont.Height;
            graphics.DrawString("---------------------------------------------", mediumFont, Brushes.Black, position, sfRTL);
            position.Y += mediumBoldFont.Height;

        }

        private void printPaymentType()
        {
            position.X = rightPage;
            graphics.DrawString("צורת תשלום  |סכום           |כמות |אחוז", mediumBoldFont, Brushes.Black, position, sfRTL);
            position.Y += 8;
            graphics.DrawString("_____________________________________________", mediumFont, Brushes.Black, position, sfRTL);
            position.Y += smallFont.Height;

            foreach (Statistic ps in XorZReport.PaymentsStatistic)
            {
                string paymentType = ps.Name;
                string total = string.Format("|₪{0:N2}", ps.TotalAmount);
                string count = "|" + ps.Count;
                string pct = string.Format("|%{0:N2}", ps.Percentage);
                graphics.DrawString(paymentType + "              " + total + "       " + count + "       " + pct + "", mediumFont, Brushes.Black, position, sfRTL);
                position.Y += mediumFont.Height;

            }
            printSumPlusTax(XorZReport.TotalAmountForReport);
        }

        private void printSumPlusTax(double sum)
        {
            double before = sum / 1.18;
            position.Y += mediumBoldFont.Height;
            position.X = rightPage;
            graphics.DrawString("לפני מעמ", mediumFont, Brushes.Black, position, sfRTL);
            position.X = 200;
            graphics.DrawString(string.Format("₪{0:N2}", before), mediumBoldFont, Brushes.Black, position, sfRTL);
            position.Y += mediumBoldFont.Height;
            position.X = rightPage;
            graphics.DrawString("מעמ", mediumFont, Brushes.Black, position, sfRTL);
            position.X = 200;
            graphics.DrawString(string.Format("₪{0:N2}", (sum - before)), mediumBoldFont, Brushes.Black, position, sfRTL);
            position.Y += mediumBoldFont.Height;
            position.X = rightPage;
            graphics.DrawString("סהכ כולל", largFont, Brushes.Black, position, sfRTL);
            position.X = 180;
            graphics.DrawString(string.Format("₪{0:N2}", sum), largFontBold, Brushes.Black, position, sfRTL);

        }

        private void fillStatistic()
        {
            position.X = rightPage;
            position.Y += XLargFont.Height;
            graphics.DrawString("סטטיסטיקה", largFontBold, Brushes.Black, position, sfRTL);
            position.Y += XLargFont.Height;
            graphics.DrawString("מחלקה    |קבוצה   |סכום     |כמות    |אחוז", mediumBoldFont, Brushes.Black, position, sfRTL);
            position.Y += smallFont.Height;
            foreach (DepartmentStatistic ds in XorZReport.DepartmentsStatistic)
            {

                graphics.DrawString("_____________________________________________", mediumFont, Brushes.Black, position, sfRTL);
                position.Y += smallFont.Height;
                graphics.DrawString(ds.Name, mediumBoldFont, Brushes.Black, position, sfRTL);
                position.X -= 120;
                graphics.DrawString(string.Format("₪{0:N2}", ds.TotalAmount), mediumBoldFont, Brushes.Black, position, sfRTL);
                position.X -= 60;
                graphics.DrawString(string.Format("{0}", ds.Count), mediumBoldFont, Brushes.Black, position, sfRTL);
                position.X -= 40;
                graphics.DrawString(string.Format("%{0:N2}", ds.Percentage), mediumBoldFont, Brushes.Black, position, sfRTL);
                position.X += 220;
                position.Y += smallFont.Height;
                graphics.DrawString("----------------------------------------------", mediumFont, Brushes.Black, position, sfRTL);
                position.Y += smallFont.Height;
                foreach (Statistic gs in ds.GroupStatistic)
                {
                    string[] words = { gs.Name, string.Format("₪{0:N2}", gs.TotalAmount), string.Format("{0}", gs.Count), string.Format("%{0:N2}", gs.Percentage), };
                    int[] tabs = { 70, 50, 60, 40 };
                    for (int i = 0; i < 4; i++)
                    {
                        position.X -= tabs[i];
                        graphics.DrawString(words[i], mediumFont, Brushes.Black, position, sfRTL);

                    }
                    position.X = rightPage;
                    position.Y += mediumBoldFont.Height;
                }
            }
            position.Y += smallFont.Height;
            graphics.DrawString("_____________________________________________", mediumFont, Brushes.Black, position, sfRTL);
            position.Y += smallFont.Height;

        }

        


        public void PrintZReport(XReportHolder report)
        {
            XorZReport = report;

            pdoc.PrintPage += new PrintPageEventHandler(pdoc_PrintZReport);
            openDialog();

        }

        private void pdoc_PrintZReport(object sender, PrintPageEventArgs e)
        {
            graphics = e.Graphics;
            fillHeader();
            fillZSetHeader();
            printPaymentType();
            fillStatistic();
            fillSummit();
        }

        private void fillZSetHeader()
        {
            position.X = center;
            position.Y += mediumBoldFont.Height;
            graphics.DrawString("Z  " + "דוח", XLargFont, Brushes.Black, position, sformat);
            position.Y += mediumBoldFont.Height;
            position.X = rightPage;
            graphics.DrawString("מספר: " + XorZReport.ZReport.ZNumber.ToString(), mediumBoldFont, Brushes.Black, position, sfRTL);
            position.Y += mediumBoldFont.Height;
            graphics.DrawString("עובד: " + XorZReport.Employee.FullName, mediumBoldFont, Brushes.Black, position, sfRTL);
            position.Y += mediumBoldFont.Height;
            graphics.DrawString("תאריך   " + DateTime.Now.ToString(), mediumBoldFont, Brushes.Black, position, sfRTL);
            position.Y += smallFont.Height;
            graphics.DrawString("---------------------------------------------", mediumFont, Brushes.Black, position, sfRTL);
            position.Y += mediumBoldFont.Height;
        }

        internal void PrintCashStatment(double sum)
        {
            cashStatment = sum;
            pdoc.PrintPage += new PrintPageEventHandler(pdoc_PrintCashStatment);
            openDialog();
        }

        private void pdoc_PrintCashStatment(object sender, PrintPageEventArgs e)
        {
            
            graphics = e.Graphics;
            position.X = center;
            position.Y += mediumBoldFont.Height;
            graphics.DrawString("הצהרת קופאי", XLargFont, Brushes.Black, position, sformat);
            position.Y += XLargFont.Height;
            position.X = rightPage;
            graphics.DrawString("סכום:", XLargFont, Brushes.Black, position, sfRTL);
            position.X -= 100;
            graphics.DrawString(string.Format("₪{0:N2}", cashStatment), XLargFont, Brushes.Black, position, sfRTL);
            position.Y += XLargFont.Height;
            position.X = rightPage;
            graphics.DrawString("תאריך   " + DateTime.Now.ToString(), mediumFont, Brushes.Black, position, sfRTL);
            position.Y += smallFont.Height;
            graphics.DrawString("---------------------------------------------", mediumFont, Brushes.Black, position, sfRTL);
            position.Y += mediumBoldFont.Height;
        }



        public void printDailyAttendances(DailyAttendance da)
        {
            pdoc.PrintPage += new PrintPageEventHandler(pdoc_PrintDailyAttendance);
            this.dailyAttendance = da;
            openDialog();
        }

        private void pdoc_PrintDailyAttendance (object sender, PrintPageEventArgs e)
        {
            graphics = e.Graphics;
            position.X = center;
            position.Y += XLargFont.Height;
            graphics.DrawString("הקלדת נוכחות", XLargFont, Brushes.Black, position, sformat);
            position.Y += largFont.Height;
            graphics.DrawString(dailyAttendance.Employee.FullName, XLargFont, Brushes.Black, position, sformat);
            position.Y += mediumFont.Height;
            position.X = rightPage;
            graphics.DrawString("כניסה: ", largFont, Brushes.Black, position, sfRTL);
            position.Y += largFont.Height + 10;
            position.X = center;
            graphics.DrawString(dailyAttendance.entryTime.ToString(), largFont, Brushes.Black, position, sformat);
            if (!dailyAttendance.isPresent)
            {
                position.Y += mediumFont.Height;
                position.X = rightPage;
                graphics.DrawString("יציאה: ", largFont, Brushes.Black, position, sfRTL);
                position.Y += largFont.Height + 10;
                position.X = center;
                graphics.DrawString(((DateTime)dailyAttendance.exitTime).ToString(), largFont, Brushes.Black, position, sformat);
            }
            position.Y += mediumFont.Height;
            graphics.DrawString("---------------------------------------------", mediumFont, Brushes.Black, position, sformat);
            position.Y += mediumBoldFont.Height;

        }

        private DailyAttendance dailyAttendance { get; set; }
    }
}