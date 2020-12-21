﻿using System;
using Marketplace.Ads.Domain.ClassifiedAds;
using Marketplace.Ads.Domain.Shared;
using Xunit;

namespace Marketplace.ClassifiedAds.Tests
{
    public class ClassifiedAd_Publish_Spec
    {
        public ClassifiedAd_Publish_Spec()
            => _classifiedAd = ClassifiedAd.Create(
                ClassifiedAdId.FromGuid(Guid.NewGuid()),
                UserId.FromGuid(Guid.NewGuid())
            );

        readonly ClassifiedAd _classifiedAd;

        [Fact]
        public void Can_publish_a_valid_ad()
        {
            _classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad"));

            _classifiedAd.UpdateText(
                ClassifiedAdText.FromString("Please buy my stuff")
            );

            _classifiedAd.UpdatePrice(
                Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup())
            );

            _classifiedAd.RequestToPublish();

            Assert.Equal(
                ClassifiedAd.ClassifiedAdState.PendingReview,
                _classifiedAd.State
            );
        }

        [Fact]
        public void Cannot_publish_with_zero_price()
        {
            _classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad"));

            _classifiedAd.UpdateText(
                ClassifiedAdText.FromString("Please buy my stuff")
            );

            _classifiedAd.UpdatePrice(
                Price.FromDecimal(0.0m, "EUR", new FakeCurrencyLookup())
            );

            Assert.Throws<DomainExceptions.InvalidEntityState>(
                () => _classifiedAd.RequestToPublish()
            );
        }

        [Fact]
        public void Cannot_publish_without_price()
        {
            _classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad"));

            _classifiedAd.UpdateText(
                ClassifiedAdText.FromString("Please buy my stuff")
            );

            Assert.Throws<DomainExceptions.InvalidEntityState>(
                () => _classifiedAd.RequestToPublish()
            );
        }

        [Fact]
        public void Cannot_publish_without_text()
        {
            _classifiedAd.SetTitle(ClassifiedAdTitle.FromString("Test ad"));

            _classifiedAd.UpdatePrice(
                Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup())
            );

            Assert.Throws<DomainExceptions.InvalidEntityState>(
                () => _classifiedAd.RequestToPublish()
            );
        }

        [Fact]
        public void Cannot_publish_without_title()
        {
            _classifiedAd.UpdateText(
                ClassifiedAdText.FromString("Please buy my stuff")
            );

            _classifiedAd.UpdatePrice(
                Price.FromDecimal(100.10m, "EUR", new FakeCurrencyLookup())
            );

            Assert.Throws<DomainExceptions.InvalidEntityState>(
                () => _classifiedAd.RequestToPublish()
            );
        }
    }
}