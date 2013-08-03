using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MusicCollection.Models;
using MusicCollection.ViewModels;
using MusicCollection.Repositories;

namespace MusicCollection.Controllers {
  public class AlbumController : Controller {
    readonly IAlbumRepository albumRepository;

    public AlbumController() : this(new AlbumRepository()) { }
    
    public AlbumController(IAlbumRepository albumRepository) {
      this.albumRepository = albumRepository;
    }

    //
    // GET: /Album/

    public ActionResult Index(int page = 1, SortType sortType = SortType.ArtistName, string filter = "") {
      var filteredAlbums = albumRepository.FindAlbumsByArtist(filter, sortType, page);
      SetMissingArtworkImage(filteredAlbums);

      var viewModel = new AlbumIndexViewModel {
        Albums = filteredAlbums,
        CurrentPage = page,
        CurrentSortType = sortType,
        FriendlySortType = sortType.ToFriendlyName(),
        NumberOfPages = albumRepository.PagesOfAlbums(),
        AlbumCount = String.IsNullOrWhiteSpace(filter) ? albumRepository.AlbumCount() : filteredAlbums.Count(),
        CurrentFilter = filter,
      };
      return View(viewModel);
    }

    void SetMissingArtworkImage(IEnumerable<Album> albumsByArtist) {
      foreach (var album in albumsByArtist) {
        if (string.IsNullOrEmpty(album.ArtworkLocation) || album.ArtworkLocation.Contains("noimage")) {
          album.ArtworkLocation = Url.Encode("Images/album-art-missing.png");
        }
      }
    }
  }
}