﻿using AzureBlobProject.Models;
using AzureBlobProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace AzureBlobProject.Controllers
{
    public class BlobController : Controller
    {
        public readonly IBlobService _blobService;

        public BlobController(IBlobService blobService)
        {
            _blobService = blobService;
        }

        [HttpGet]
        public async Task<IActionResult> Manage(string containerName)
        {
            var blobObject = await _blobService.GetAllBlobs(containerName);
            return View(blobObject);
        }

        [HttpGet]
        public async Task<IActionResult> AddFile(string containerName)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFile(string containerName, IFormFile file)
        {
            var fileName = Path.GetFileNameWithoutExtension(file.FileName + "_" + Guid.NewGuid() + Path.GetExtension(file.FileName));

            var result = await _blobService.CreateBlob(fileName, file, containerName, new BlobModel());

            if (result)
                return RedirectToAction("Index", "Container");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ViewFile(string name, string containerName)
        {
            return Redirect(await _blobService.GetBlob(name, containerName));
        }

        public async Task<IActionResult> DeleteFile(string name, string containerName)
        {
            await _blobService.DeleteBlob(name, containerName);

            return RedirectToAction("Manage", new { containerName });
        }
    }
}
