using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using System.ComponentModel;

namespace team_20241228.Models
{
		public class CTPurchaseWrap
		{
			//use gobal variable
			private TPurchase _purchase;


			//建立變數
			//--------------------------------------
			public TPurchase purchase
			{
				get { return _purchase; }
				set { _purchase = value; }
			}
			public CTPurchaseWrap()
			{
				_purchase = new TPurchase();
			}

			//-----------欄位定義---------------------------------------

			[DisplayName("採購單號")]
			public int FId
			{

				get { return _purchase.FId; }
				set { _purchase.FId = value; }
			}

			[DisplayName("採購日")]
			public DateOnly? FPurchase_date
			{

				get { return _purchase.FBuyDate ; }
				set { _purchase.FBuyDate = value ; }
			}

			[DisplayName("供應商id")]
			public string? FSupplier_id
			{
				get { return _purchase.FSupplierId; }
				set { _purchase.FSupplierId = value; }
			}

			[DisplayName("供應商名稱")]
			public string? FSupplier_name
			{
				get { return _purchase.FSupplierName; }
				set { _purchase.FSupplierName = value; }
			}

			[DisplayName("採購人")]
			public string? FPurchase_person
			{
				get { return _purchase.FBuyer; }
				set { _purchase.FBuyer = value; }
			}

			[DisplayName("有效期限")]
			public DateOnly? FValid_date
			{
				get { return _purchase.FExpirationDate ; }
				set { _purchase.FExpirationDate = value ; }
			}


			[DisplayName("應稅")]
			public string? FTax_type
			{
				get { return _purchase.FIsTax; }
				set { _purchase.FIsTax = value ; }
			}

			[DisplayName("發票格式")]
			public string? FInvoice_form
			{
				get { return _purchase.FInvoiceForm; }
				set { _purchase.FInvoiceForm = value; }
			}
			[DisplayName("支付方式")]
			public string? FPayment
			{
				get { return _purchase.FPayment; }
				set { _purchase.FPayment = value; }
			}
			[DisplayName("建檔人")]
			public string? FCeate_file_person
			{
				get { return _purchase.FCreater; }
				set { _purchase.FCreater = value; }
			}

			[DisplayName("修改人")]
			public string? FModify_person
			{
				get { return _purchase.FEditor; }
				set { _purchase.FEditor = value; }
			}
			[DisplayName("採購金額")]
			public int? FPurchase_dollar
			{
				get { return _purchase.FPreTax; }
				set { _purchase.FPreTax = value; }
			}

			[DisplayName("稅")]
			public int? FDollar_tax
			{
				get { return _purchase.FTax; }
				set { _purchase.FTax = value ; }
			}

			[DisplayName("總額")]
			public int? FTotal_dollar
			{
				get { return _purchase.FTotal; }
				set { _purchase.FTotal = value ; }
			}

			[DisplayName("備註")]
			public string? FNote
			{
				get { return _purchase.FNote; }
				set { _purchase.FNote = value; }
			}
		}
}
