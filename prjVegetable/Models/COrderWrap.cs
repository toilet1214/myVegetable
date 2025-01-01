﻿using System.ComponentModel;

namespace prjVegetable.Models
{
    public class COrderWrap
    {
        private TOrder _order = null;
        public TOrder order
        {
            get { return _order; }
            set { _order = value; }
        }
        public COrderWrap()
        {
            _order = new TOrder();
        }
        public int FId
        {
            get { return _order.FId; }
            set { _order.FId = value; }
        }
        [DisplayName("會員ID")]
        public int FBuyerId
        {
            get { return _order.FBuyerId; }
            set { _order.FBuyerId = value; }
        }
        [DisplayName("總金額")]
        public int FTotal
        {
            get { return _order.FTotal; }
            set { _order.FTotal = value; }
        }
        [DisplayName("訂單狀態")]
        public string FStatus
        {
            get { return _order.FStatus; }
            set { _order.FStatus = value; }
        }
        [DisplayName("下單日期")]
        public DateTime FOrderAt
        {
            get { return _order.FOrderAt; }
            set { _order.FOrderAt = value; }
        }
        [DisplayName("收件地址")]
        public string FAddress
        {
            get { return _order.FAddress; }
            set { _order.FAddress = value; }
        }
        [DisplayName("收件人")]
        public string FReceiverName
        {
            get { return _order.FReceiverName; }
            set { _order.FReceiverName = value; }
        }
        [DisplayName("連絡電話")]
        public string FPhone
        {
            get { return _order.FPhone; }
            set { _order.FPhone = value; }
        }
        [DisplayName("備註")]
        public string FRemark
        {
            get { return _order.FRemark; }
            set { _order.FRemark = value; }
        }
    }
}
