﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Repository.Models;
using Repository.Models.DatabaseModels;
using Repository.ViewModels;

namespace Repository.Controllers
{
  [Authorize(Roles = "Admin")]
  public class RolesController : Controller
  {
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<User> _userManager;

    public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
    {
      _roleManager = roleManager;
      _userManager = userManager;
    }

    public IActionResult Index() => View(_roleManager.Roles.ToList());

    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(string name)
    {
      if (string.IsNullOrEmpty(name)) return View(name);
      var result = await _roleManager.CreateAsync(new IdentityRole(name));
      if (result.Succeeded)
      {
        return RedirectToAction("Index");
      }
      else
      {
        foreach (var error in result.Errors)
        {
          ModelState.AddModelError(string.Empty, error.Description);
        }
      }

      return View(name);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string id)
    {
      var role = await _roleManager.FindByIdAsync(id);
      if (role != null)
      {
        IdentityResult result = await _roleManager.DeleteAsync(role);
      }

      return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(string userId)
    {
      // получаем пользователя
      var user = await _userManager.FindByIdAsync(userId);
      if (user == null) return NotFound();
      // получем список ролей пользователя
      var userRoles = await _userManager.GetRolesAsync(user);
      var allRoles = _roleManager.Roles.ToList();
      var model = new ChangeRoleViewModel
      {
        UserId = user.Id,
        UserEmail = user.Email,
        UserRoles = userRoles,
        AllRoles = allRoles
      };
      return View(model);

    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string userId, List<string> roles)
    {
      // получаем пользователя
      var user = await _userManager.FindByIdAsync(userId);
      if (user == null) return NotFound();
      // получем список ролей пользователя
      var userRoles = await _userManager.GetRolesAsync(user);
      // получаем все роли
      var allRoles = _roleManager.Roles.ToList();
      // получаем список ролей, которые были добавлены
      var addedRoles = roles.Except(userRoles);
      // получаем роли, которые были удалены
      var removedRoles = userRoles.Except(roles);

      await _userManager.AddToRolesAsync(user, addedRoles);

      await _userManager.RemoveFromRolesAsync(user, removedRoles);

      return RedirectToAction("Index","Users");

    }
  }
}