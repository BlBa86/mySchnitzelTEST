﻿namespace GCloud.Shared.Dto.Domain.CouponUsageAction
{
    public class InvoiceDiscountUsageActionDto : AbstractUsageActionDto
    {
        public decimal Discount { get; set; }
        public CouponTypeDto? DiscountType { get; set; }
        public override bool Apply(AbstractUsageActionVisitor visitor, CouponDto couponDto)
        {
            return visitor.Visit(this, couponDto);
        }
    }
}