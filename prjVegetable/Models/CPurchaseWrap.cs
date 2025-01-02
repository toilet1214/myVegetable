using Microsoft.AspNetCore.Mvc;
using prjVegetable.Models;
using System.ComponentModel;

namespace prjVegetable.Models
{
		public class CPurchaseWrap
		{
			//use gobal variable
			private TPurchase _Purchase;


			//建立變數
			//--------------------------------------
			public TPurchase Purchase
			{
				get { return _Purchase; }
				set { _Purchase = value; }
			}
			public CPurchaseWrap()
			{
				_Purchase = new TPurchase();
			}

			//-----------欄位定義---------------------------------------

			[DisplayName("採購單號")]
			public int FId
			{

				get { return _Purchase.FId; }
				set { _Purchase.FId = value; }
			}

			[DisplayName("採購日")]
			public DateOnly? FBuyDate
        {

				get { return _Purchase.FBuyDate ; }
				set { _Purchase.FBuyDate = value ; }
			}

			[DisplayName("供應商id")]
			public string? FSupplierId
        {
				get { return _Purchase.FSupplierId; }
				set { _Purchase.FSupplierId = value; }
			}

			[DisplayName("供應商名稱")]
			public string? FSupplierName
        {
				get { return _Purchase.FSupplierName; }
				set { _Purchase.FSupplierName = value; }
			}

			[DisplayName("採購人")]
			public string? FBuyer
        {
				get { return _Purchase.FBuyer; }
				set { _Purchase.FBuyer = value; }
			}

			[DisplayName("有效期限")]
			public DateOnly? FExpirationDate
        {
				get { return _Purchase.FExpirationDate ; }
				set { _Purchase.FExpirationDate = value ; }
			}


			[DisplayName("應稅")]
			public string? FIsTax
        {
				get { return _Purchase.FIsTax; }
				set { _Purchase.FIsTax = value ; }
			}

			[DisplayName("發票格式")]
			public string? FInvoiceForm
        {
				get { return _Purchase.FInvoiceForm; }
				set { _Purchase.FInvoiceForm = value; }
			}
			[DisplayName("支付方式")]
			public string? FPayment
			{
				get { return _Purchase.FPayment; }
				set { _Purchase.FPayment = value; }
			}
			[DisplayName("建檔人")]
			public string? FCreater
        {
				get { return _Purchase.FCreater; }
				set { _Purchase.FCreater = value; }
			}

			[DisplayName("修改人")]
			public string? FEditor
        {
				get { return _Purchase.FEditor; }
				set { _Purchase.FEditor = value; }
			}
			[DisplayName("採購金額")]
			public int? FPreTax
        {
				get { return _Purchase.FPreTax; }
				set { _Purchase.FPreTax = value; }
			}

			[DisplayName("稅")]
			public int? FTax
        {
				get { return _Purchase.FTax; }
				set { _Purchase.FTax = value ; }
			}

			[DisplayName("總額")]
			public int? FTotal
        {
				get { return _Purchase.FTotal; }
				set { _Purchase.FTotal = value ; }
			}

			[DisplayName("備註")]
			public string? FNote
			{
				get { return _Purchase.FNote; }
				set { _Purchase.FNote = value; }
			}
		}
}
