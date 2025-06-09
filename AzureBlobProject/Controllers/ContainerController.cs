﻿using AzureBlobProject.Models;
using AzureBlobProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace AzureBlobProject.Controllers
{
    public class ContainerController : Controller
    {
        public readonly IContainerService _containerService;

        public ContainerController(IContainerService containerService) 
        {
            _containerService = containerService;
        }


        public async Task<IActionResult> Index()
        {
            var allContainer = await _containerService.GetAllContainerAsync();
            return View(allContainer);
        }

        public async Task<IActionResult> Create()
        {
            return View(new ContainerModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(ContainerModel container)
        {
            await _containerService.CreateContainersAsync(container.Name);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(string containerName)
        {
            await _containerService.DeleteContainersAsync(containerName);
            return RedirectToAction(nameof(Index));
        }
    }
}
