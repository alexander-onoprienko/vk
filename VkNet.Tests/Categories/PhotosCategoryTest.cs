﻿using System;
using System.Collections.ObjectModel;
using FluentNUnit;
using Moq;
using NUnit.Framework;
using VkNet.Categories;
using VkNet.Model;
using VkNet.Model.Attachments;
using VkNet.Utils;

namespace VkNet.Tests.Categories
{
    [TestFixture]
    public class PhotosCategoryTest
    {
        public PhotoCategory GetMockedPhotosCategory(string url, string json)
        {
            var browser = Mock.Of<IBrowser>(m => m.GetJson(url) == json);
            return new PhotoCategory(new VkApi{Browser = browser, AccessToken = "token"});
        }

        #region GetProfileUploadServer
        [Test]
        public void GetProfileUploadServer_NormalCase()
        {
            const string url = "https://api.vk.com/method/photos.getProfileUploadServer?v=5.9&access_token=token";
            const string json =
                @"{
                    'response': {
                      'upload_url': 'http://cs618026.vk.com/upload.php?_query=eyJhY3QiOiJvd25lcl9waG90byIsInNh'
                    }
                  }";

            UploadServerInfo info = GetMockedPhotosCategory(url, json).GetProfileUploadServer();

            info.UploadUrl.ShouldEqual("http://cs618026.vk.com/upload.php?_query=eyJhY3QiOiJvd25lcl9waG90byIsInNh");
        }
        #endregion

        #region GetMessagesUploadServer
        [Test]
        public void GetMessagesUploadServer_NormalCase()
        {
            const string url = "https://api.vk.com/method/photos.getMessagesUploadServer?v=5.9&access_token=token";
            const string json =
                @"{
                    'response': {
                      'upload_url': 'http://cs618026.vk.com/upload.php?act=do_add&mid=234695118&aid=-3&gid=0&hash=de2523dd173af592a5dcea351a0ea9e7&rhash=71534021af2730c5b88c05d9ca7c9ed3&swfupload=1&api=1&mailphoto=1',
                      'album_id': -3,
                      'user_id': 234618
                    }
                  }";

            UploadServerInfo info = GetMockedPhotosCategory(url, json).GetMessagesUploadServer();

            info.UploadUrl.ShouldEqual("http://cs618026.vk.com/upload.php?act=do_add&mid=234695118&aid=-3&gid=0&hash=de2523dd173af592a5dcea351a0ea9e7&rhash=71534021af2730c5b88c05d9ca7c9ed3&swfupload=1&api=1&mailphoto=1");
            info.AlbumId.ShouldEqual(-3);
            info.UserId.ShouldEqual(234618);
        }
        #endregion

        #region CreateAlbum

        [Test]
        public void CreateAlbum_NormalCase()
        {
            const string url = "https://api.vk.com/method/photos.createAlbum?title=hello world&description=description for album&v=5.9&access_token=token";
            const string json =
                @"{
                    'response': {
                      'id': 197266686,
                      'thumb_id': -1,
                      'owner_id': 234698,
                      'title': 'hello world',
                      'description': 'description for album',
                      'created': 1403185184,
                      'updated': 1403185184,
                      'privacy': 0,
                      'comment_privacy': 0,
                      'size': 0
                    }
                  }";

            PhotoAlbum album = GetMockedPhotosCategory(url, json)
                .CreateAlbum(title: "hello world", description: "description for album");

            album.Id.ShouldEqual(197266686);
            album.ThumbId.ShouldEqual(-1);
            album.OwnerId.ShouldEqual(234698);
            album.Title.ShouldEqual("hello world");
            album.Description.ShouldEqual("description for album");
            album.Created.ShouldEqual(new DateTime(2014, 6, 19, 17, 39, 44));
            album.Updated.ShouldEqual(new DateTime(2014, 6, 19, 17, 39, 44));
            album.Privacy.ShouldEqual(0);
            album.CommentPrivacy.ShouldEqual(0);
            album.Size.ShouldEqual(0);
        }

        #endregion

        #region EditAlbum
        [Test]
        public void EditAlbum_NormalCase()
        {
            const string url = "https://api.vk.com/method/photos.editAlbum?album_id=19726&title=new album title&description=new description&v=5.9&access_token=token";


            const string json =
                @"{
                    'response': 1
                  }";

            bool result = GetMockedPhotosCategory(url, json).EditAlbum(19726, "new album title", "new description");
            result.ShouldBeTrue();
        }
        #endregion

        #region GetAlbums
        [Test]
        public void GetAlbums_NormalCase()
        {
            const string url = "https://api.vk.com/method/photos.getAlbums?owner_id=1&v=5.9&access_token=token";
            const string json =
                @"{
                    'response': {
                      'count': 1,
                      'items': [
                        {
                          'id': 136592355,
                          'thumb_id': 321112194,
                          'owner_id': 1,
                          'title': 'Здесь будут новые фотографии для прессы-службы',
                          'description': '',
                          'created': 1307628778,
                          'updated': 1398625473,
                          'size': 8
                        }
                      ]
                    }
                  }";

            ReadOnlyCollection<PhotoAlbum> albums = GetMockedPhotosCategory(url, json).GetAlbums(1);
            albums.Count.ShouldEqual(1);

            albums[0].Id.ShouldEqual(136592355);
            albums[0].ThumbId.ShouldEqual(321112194);
            albums[0].OwnerId.ShouldEqual(1);
            albums[0].Title.ShouldEqual("Здесь будут новые фотографии для прессы-службы");
            albums[0].Description.ShouldEqual(string.Empty);
	        albums[0].Created.ShouldEqual(new DateTime(2011, 6, 9, 14, 12, 58, DateTimeKind.Utc).ToLocalTime());
           	albums[0].Updated.ShouldEqual(new DateTime(2014, 4, 27, 19, 4, 33).ToLocalTime());
            albums[0].Size.ShouldEqual(8);
        }
        #endregion

        #region GetAlbumsCount
        [Test]
        public void GetAlbumsCount_NormalCase()
        {
            const string url = "https://api.vk.com/method/photos.getAlbumsCount?user_id=1&v=5.9&access_token=token";
            const string json =
                @"{
                    'response': 1
                  }";

            int count = GetMockedPhotosCategory(url, json).GetAlbumsCount(1);
            count.ShouldEqual(1);
        }
        #endregion

        #region DeleteAlbum
        [Test]
        public void DeleteAlbum_NormalCase()
        {
            const string url = "https://api.vk.com/method/photos.deleteAlbum?album_id=197303&v=5.9&access_token=token";
            const string json =
                @"{
                    'response': 1
                  }";

            bool result = GetMockedPhotosCategory(url, json).DeleteAlbum(197303);
            result.ShouldBeTrue();
        }
        #endregion

        #region GetProfile
        [Test]
        public void GetProfile_NormalCase()
        {
            const string url = "https://api.vk.com/method/photos.getProfile?owner_id=1&rev=1&extended=1&count=2&offset=3&v=5.9&access_token=token";
            const string json =
                @"{
                    'response': {
                      'count': 7,
                      'items': [
                        {
                          'id': 278184324,
                          'album_id': -6,
                          'owner_id': 1,
                          'photo_75': 'http://cs10408.vk.me/u4172580/-6/s_24887a5a.jpg',
                          'photo_130': 'http://cs10408.vk.me/u4172580/-6/m_79ab6f4a.jpg',
                          'photo_604': 'http://cs10408.vk.me/u4172580/-6/x_ee97448e.jpg',
                          'text': '',
                          'date': 1328126422,
                          'post_id': 45430,
                          'likes': {
                            'user_likes': 0,
                            'count': 471203
                          },
                          'comments': {
                            'count': 1
                          },
                          'can_comment': 0,
                          'tags': {
                            'count': 0
                          }
                        },
                        {
                          'id': 263219735,
                          'album_id': -6,
                          'owner_id': 1,
                          'photo_75': 'http://cs9591.vk.me/u00001/136592355/s_39db64b7.jpg',
                          'photo_130': 'http://cs9591.vk.me/u00001/136592355/m_5f3fd6ac.jpg',
                          'photo_604': 'http://cs9591.vk.me/u00001/136592355/x_d51dbfac.jpg',
                          'photo_807': 'http://cs9591.vk.me/u00001/136592355/y_8cc51452.jpg',
                          'photo_1280': 'http://cs9591.vk.me/u00001/136592355/z_90874cc2.jpg',
                          'photo_2560': 'http://cs9591.vk.me/u00001/136592355/w_f6a60338.jpg',
                          'text': '',
                          'date': 1307883759,
                          'likes': {
                            'user_likes': 0,
                            'count': 670292
                          },
                          'comments': {
                            'count': 6
                          },
                          'can_comment': 0,
                          'tags': {
                            'count': 0
                          }
                        }
                      ]
                    }
                  }";

            ReadOnlyCollection<Photo> photos = GetMockedPhotosCategory(url, json).GetProfile(ownerId: 1, offset: 3, rev: true, count: 2, extended:true);
            photos.Count.ShouldEqual(2);
            photos[0].Id.ShouldEqual(278184324);
            photos[0].PostId.ShouldEqual(45430);
            photos[0].Likes.Count.ShouldEqual(471203);
            photos[0].Likes.UserLikes.ShouldEqual(false);
            photos[0].Comments.Count.ShouldEqual(1);
            photos[0].CanComment.ShouldEqual(false);
            photos[0].Tags.Count.ShouldEqual(0);

        }
        #endregion

        #region GetAll
        [Test]
        public void GetAll_NormalCase()
        {
            const string url = "https://api.vk.com/method/photos.getAll?owner_id=1&count=2&offset=4&v=5.9&access_token=token";
            const string json =
                @"{
                    'response': {
                      'count': 173,
                      'items': [
                        {
                          'id': 328693256,
                          'album_id': -7,
                          'owner_id': 1,
                          'photo_75': 'http://cs7004.vk.me/c7006/v7006001/26e37/xOF6D9lY3CU.jpg',
                          'photo_130': 'http://cs7004.vk.me/c7006/v7006001/26e38/3atNlPEJpaA.jpg',
                          'photo_604': 'http://cs7004.vk.me/c7006/v7006001/26e39/OfHtSC9qtuA.jpg',
                          'photo_807': 'http://cs7004.vk.me/c7006/v7006001/26e3a/el6ZcXa9WSc.jpg',
                          'width': 609,
                          'height': 574,
                          'text': 'Сегодня должности раздаются чиновниками, которые боятся конкуренции и подбирают себе все менее талантливых и все более беспомощных подчиненных. Государственные посты должны распределяться на основе прозрачных механизмов, в том числе, прямых выборов.',
                          'date': 1398658327
                        },
                        {
                          'id': 328693245,
                          'album_id': -7,
                          'owner_id': 1,
                          'photo_75': 'http://cs7004.vk.me/c7006/v7006001/26e2f/sVIvq64s9N8.jpg',
                          'photo_130': 'http://cs7004.vk.me/c7006/v7006001/26e30/IeqoOkYl7Xw.jpg',
                          'photo_604': 'http://cs7004.vk.me/c7006/v7006001/26e31/ia2se1JpNi0.jpg',
                          'photo_807': 'http://cs7004.vk.me/c7006/v7006001/26e32/bpijpqfjhyw.jpg',
                          'width': 609,
                          'height': 543,
                          'text': 'Текущее обилие противоречащих друг другу законов стимулирует коррупцию и замедляет экономический рост. Страна нуждается в отмене большей части законотворческого балласта, принятого за последние 10 лет.',
                          'date': 1398658302
                        }
                      ]
                    }
                  }";

            ReadOnlyCollection<Photo> photos = GetMockedPhotosCategory(url, json).GetAll(ownerId: 1, offset: 4, count: 2);
            photos.Count.ShouldEqual(2);

            photos[0].Id.ShouldEqual(328693256);
            photos[0].AlbumId.ShouldEqual(-7);
            photos[0].OwnerId.ShouldEqual(1);
            photos[0].Photo75.ShouldEqual(new Uri("http://cs7004.vk.me/c7006/v7006001/26e37/xOF6D9lY3CU.jpg"));
            photos[0].Photo130.ShouldEqual(new Uri("http://cs7004.vk.me/c7006/v7006001/26e38/3atNlPEJpaA.jpg"));
            photos[0].Photo604.ShouldEqual(new Uri("http://cs7004.vk.me/c7006/v7006001/26e39/OfHtSC9qtuA.jpg"));
            photos[0].Photo807.ShouldEqual(new Uri("http://cs7004.vk.me/c7006/v7006001/26e3a/el6ZcXa9WSc.jpg"));
            photos[0].Width.ShouldEqual(609);
            photos[0].Height.ShouldEqual(574);
            photos[0].Text.ShouldEqual("Сегодня должности раздаются чиновниками, которые боятся конкуренции и подбирают себе все менее талантливых и все более беспомощных подчиненных. Государственные посты должны распределяться на основе прозрачных механизмов, в том числе, прямых выборов.");
            photos[0].CreateTime.ShouldEqual(new DateTime(2014, 4, 28, 8, 12, 7));
        }

#endregion

        #region Search
        [Test]
        public void Search_NormalCase()
        {
            const string url = "https://api.vk.com/method/photos.search?q=порно&offset=2&count=3&v=5.9&access_token=token";
            const string json =
                @"{
                    'response': {
                      'count': 48888,
                      'items': [
                        {
                          'id': 331520481,
                          'album_id': 182104020,
                          'owner_id': -49512556,
                          'user_id': 100,
                          'photo_75': 'http://cs620223.vk.me/v620223385/bd1f/SajcsJOh7hk.jpg',
                          'photo_130': 'http://cs620223.vk.me/v620223385/bd20/85-Qkc4oNH8.jpg',
                          'photo_604': 'http://cs620223.vk.me/v620223385/bd21/88vFsC-Z_FE.jpg',
                          'photo_807': 'http://cs620223.vk.me/v620223385/bd22/YqRauv0neMY.jpg',
                          'width': 807,
                          'height': 515,
                          'text': '🍓 [club49512556|ЗАХОДИ К НАМ]\nчастное фото секси обнаженные девочки малолетки порно голые сиськи попки эротика няша шлюха грудь секс instagirls instagram лето\n#секс #девушки #девочки #instagram #instagirls #няша #InstaSize #лето #ПОПКИ',
                          'date': 1403455788
                        },
                        {
                          'id': 332606009,
                          'album_id': -7,
                          'owner_id': 178964623,
                          'photo_75': 'http://cs618519.vk.me/v618519623/9595/RvC4OjMXsSM.jpg',
                          'photo_130': 'http://cs618519.vk.me/v618519623/9596/AGp73aAvQo0.jpg',
                          'photo_604': 'http://cs618519.vk.me/v618519623/9597/LRsFBCik5t0.jpg',
                          'photo_807': 'http://cs618519.vk.me/v618519623/9598/Qtge80swvSs.jpg',
                          'photo_1280': 'http://cs618519.vk.me/v618519623/9599/824w0bo3RAQ.jpg',
                          'width': 768,
                          'height': 1024,
                          'text': 'порно',
                          'date': 1403442663
                        },
                        {
                          'id': 331193616,
                          'album_id': 197460133,
                          'owner_id': 32396848,
                          'photo_75': 'http://cs620628.vk.me/v620628848/954d/NB9R43nYW_E.jpg',
                          'photo_130': 'http://cs620628.vk.me/v620628848/954e/0KLMGHdB2RA.jpg',
                          'photo_604': 'http://cs620628.vk.me/v620628848/954f/U7FTHERNKPU.jpg',
                          'photo_807': 'http://cs620628.vk.me/v620628848/9550/eGywWT4JZ20.jpg',
                          'photo_1280': 'http://cs620628.vk.me/v620628848/9551/AS2EFpUEY_4.jpg',
                          'width': 1280,
                          'height': 720,
                          'text': 'порно xD',
                          'date': 1403442409
                        }
                      ]
                    }
                  }";

            ReadOnlyCollection<Photo> photos = GetMockedPhotosCategory(url, json).Search(query: "порно", offset:2, count:3);

            photos.Count.ShouldEqual(3);

            photos[0].Id.ShouldEqual(331520481);
            photos[0].AlbumId.ShouldEqual(182104020);
            photos[0].OwnerId.ShouldEqual(-49512556);
            photos[0].UserId.ShouldEqual(100);
            photos[0].Photo75.ShouldEqual(new Uri("http://cs620223.vk.me/v620223385/bd1f/SajcsJOh7hk.jpg"));
            photos[0].Photo130.ShouldEqual(new Uri("http://cs620223.vk.me/v620223385/bd20/85-Qkc4oNH8.jpg"));
            photos[0].Photo604.ShouldEqual(new Uri("http://cs620223.vk.me/v620223385/bd21/88vFsC-Z_FE.jpg"));
            photos[0].Photo807.ShouldEqual(new Uri("http://cs620223.vk.me/v620223385/bd22/YqRauv0neMY.jpg"));
            photos[0].Width.ShouldEqual(807);
            photos[0].Height.ShouldEqual(515);
            photos[0].Text.ShouldEqual("🍓 [club49512556|ЗАХОДИ К НАМ]\nчастное фото секси обнаженные девочки малолетки порно голые сиськи попки эротика няша шлюха грудь секс instagirls instagram лето\n#секс #девушки #девочки #instagram #instagirls #няша #InstaSize #лето #ПОПКИ");
            photos[0].CreateTime.ShouldEqual(new DateTime(2014, 6, 22, 20, 49, 48));  //  2014-06-22 20:49:48.000
        }

        [Test]
        public void Search_Error26_Lat_and_Long_in_output_photo()
        {
            const string url = "https://api.vk.com/method/photos.search?lat=30&long=30&count=2&v=5.9&access_token=token";
            const string json =
                @"{
                    'response': {
                      'count': 12,
                      'items': [
                        {
                          'id': 334408466,
                          'album_id': 198144854,
                          'owner_id': 258913887,
                          'photo_75': 'http://cs617419.vk.me/v617419887/11e90/GD__Lv5FTI4.jpg',
                          'photo_130': 'http://cs617419.vk.me/v617419887/11e91/f-4hN1xff9I.jpg',
                          'photo_604': 'http://cs617419.vk.me/v617419887/11e92/KiTWG4Lk8sE.jpg',
                          'photo_807': 'http://cs617419.vk.me/v617419887/11e93/LXbjRssgtso.jpg',
                          'width': 640,
                          'height': 640,
                          'text': '',
                          'date': 1404294037,
                          'lat': 29.999996,
                          'long': 29.999997
                        },
                        {
                          'id': 326991086,
                          'album_id': -6,
                          'owner_id': 249390767,
                          'photo_75': 'http://cs605216.vk.me/v605216767/5336/XeqYTC3wgwo.jpg',
                          'photo_130': 'http://cs605216.vk.me/v605216767/5337/IdbmUgGaoys.jpg',
                          'photo_604': 'http://cs605216.vk.me/v605216767/5338/6wIHGv9_xZ8.jpg',
                          'width': 403,
                          'height': 336,
                          'text': '',
                          'date': 1396601780,
                          'lat': 29.942251,
                          'long': 29.882819,
                          'post_id': 1
                        }
                      ]
                    }
                  }";

            var photos = GetMockedPhotosCategory(url, json).Search(query: "", lat: 30, longitude: 30, count: 2);

            photos.Count.ShouldEqual(2);

            photos[0].Latitude.ShouldEqual(29.999996185302734);
            photos[0].Longitude.ShouldEqual(29.999996185302734);

            photos[1].Latitude.ShouldEqual(29.942251205444336);
            photos[1].Longitude.ShouldEqual(29.882818222045898);
        }
#endregion

        #region SaveWallPhoto
        [Test]
        public void SaveWallPhoto_NormalCase()
        {
            const string url = @"https://api.vk.com/method/photos.saveWallPhoto?user_id=1234&group_id=123&photo=photo&server=5678&hash=hash_hash&v=5.9&access_token=token";
            const string json = @"{
    'response': [
        {
            'id': 3446123,
            'album_id': -12,
            'owner_id': 234695890,
            'photo_75': 'http://cs7004.vk.me/c625725/v625725118/8c39/XZJpyifpfkM.jpg',
            'photo_130': 'http://cs7004.vk.me/c625725/v625725118/8c3a/cYyzeNiQCwg.jpg',
            'photo_604': 'http://cs7004.vk.me/c625725/v625725118/8c3b/b9rHdTFfLuw.jpg',
            'photo_807': 'http://cs7004.vk.me/c625725/v625725118/8c3c/POYM67dCGZg.jpg',
            'photo_1280': 'http://cs7004.vk.me/c625725/v625725118/8c3d/OWWWGO1gkOI.jpg',
            'width': 1256,
            'height': 320,
            'text': '',
            'date': 1415629651
        }
    ]
}";

            var result = GetMockedPhotosCategory(url, json).SaveWallPhoto("photo", 1234, 123, 5678, "hash_hash");

            result.Count.ShouldEqual(1);

            Photo photo = result[0];
            photo.ShouldNotBeNull();
            photo.Id.ShouldEqual(3446123);
            photo.AlbumId.ShouldEqual(-12);
            photo.OwnerId.ShouldEqual(234695890);
            photo.Photo75.ShouldEqual(new Uri("http://cs7004.vk.me/c625725/v625725118/8c39/XZJpyifpfkM.jpg"));
            photo.Photo130.ShouldEqual(new Uri("http://cs7004.vk.me/c625725/v625725118/8c3a/cYyzeNiQCwg.jpg"));
            photo.Photo604.ShouldEqual(new Uri("http://cs7004.vk.me/c625725/v625725118/8c3b/b9rHdTFfLuw.jpg"));
            photo.Photo807.ShouldEqual(new Uri("http://cs7004.vk.me/c625725/v625725118/8c3c/POYM67dCGZg.jpg"));
            photo.Photo1280.ShouldEqual(new Uri("http://cs7004.vk.me/c625725/v625725118/8c3d/OWWWGO1gkOI.jpg"));
            photo.Width.ShouldEqual(1256);
            photo.Height.ShouldEqual(320);
            photo.Text.ShouldEqual(string.Empty);
            photo.CreateTime.ShouldEqual(new DateTime(2014, 11, 10, 17, 27, 31));
        }
        #endregion

    }
}