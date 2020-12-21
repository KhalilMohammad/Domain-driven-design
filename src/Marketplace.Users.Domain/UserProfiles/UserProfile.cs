using System;
using Marketplace.EventSourcing;
using static Marketplace.Users.Domain.UserProfiles.Events;

namespace Marketplace.Users.Domain.UserProfiles
{
    public class UserProfile : AggregateRoot
    {
        public static UserProfile Create(
            UserId id,
            FullName fullName,
            DisplayName displayName)
        {
            var profile = new UserProfile();

            profile.Apply(
                new V1.UserRegistered
                {
                    UserId = id,
                    FullName = fullName,
                    DisplayName = displayName
                }
            );
            return profile;
        }

        // Aggregate state properties
        FullName FullName { get; set; }
        DisplayName DisplayName { get; set; }
        string PhotoUrl { get; set; }

        public void UpdateFullName(FullName fullName)
            => Apply(
                new V1.UserFullNameUpdated
                {
                    UserId = Id,
                    FullName = fullName
                }
            );

        public void UpdateDisplayName(DisplayName displayName)
            => Apply(
                new V1.UserDisplayNameUpdated
                {
                    UserId = Id,
                    DisplayName = displayName
                }
            );

        public void UpdateProfilePhoto(Uri photoUrl)
            => Apply(
                new V1.ProfilePhotoUploaded
                {
                    UserId = Id,
                    PhotoUrl = photoUrl.ToString()
                }
            );

        protected override void When(object @event)
        {
            switch (@event)
            {
                case V1.UserRegistered e:
                    Id = e.UserId;
                    FullName = new FullName(e.FullName);
                    DisplayName = new DisplayName(e.DisplayName);
                    break;
                case V1.UserFullNameUpdated e:
                    FullName = new FullName(e.FullName);
                    break;
                case V1.UserDisplayNameUpdated e:
                    DisplayName = new DisplayName(e.DisplayName);
                    break;
                case V1.ProfilePhotoUploaded e:
                    PhotoUrl = e.PhotoUrl;
                    break;
            }
        }

        protected override void EnsureValidState() { }
    }
}