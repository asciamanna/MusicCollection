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
    const int resultsPerPage = 56;
    readonly IAlbumRepository albumRepository;

    public AlbumController() : this(new AlbumRepository()) { }
    
    public AlbumController(IAlbumRepository albumRepository) {
      this.albumRepository = albumRepository;
    }

    //
    // GET: /Album/

    public ActionResult Index(int page = 1, SortType sortType = SortType.ArtistName, string filter = "") {
      var filteredAlbums = albumRepository.FindAlbumsByArtist(filter, sortType);
      var numberOfAlbums = filteredAlbums.Count();

      SetMissingArtworkImage(filteredAlbums);

      var viewModel = new AlbumIndexViewModel {
        Albums = PageAlbums(filteredAlbums, page),
        CurrentPage = page,
        CurrentSortType = sortType,
        FriendlySortType = sortType.ToFriendlyName(),
        NumberOfPages = CalculateNumberOfPages(numberOfAlbums),
        AlbumCount = numberOfAlbums,
        CurrentFilter = filter,
      };

      return View(viewModel);
    }

    List<Album> PageAlbums(List<Album> filteredAlbums, int page) {
      return filteredAlbums.Skip(resultsPerPage * (page - 1)).Take(resultsPerPage).ToList();
    }

    void SetMissingArtworkImage(IEnumerable<Album> albumsByArtist) {
      foreach (var album in albumsByArtist) {
        if (string.IsNullOrEmpty(album.ArtworkLocation) || album.ArtworkLocation.Contains("noimage")) {
          album.ArtworkLocation = Url.Encode("Images/album-art-missing.png");
        }
      }
    }

    int CalculateNumberOfPages(int totalAlbums) {
      var pages = totalAlbums / resultsPerPage;
      if (totalAlbums % resultsPerPage != 0) pages++;
      return pages;
    }
  }
}