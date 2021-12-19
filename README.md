# Project

Online shop implementation for Okredo task.

Includes:
* Category caching.
* Full-Text search for product names.
* Two sepparate order strategies for registered/unregistered users. If user is not logged in, user is identified by cartCookie value from request header.
* Optimistic concurrency for product quantity. If multiple users manage to order same product with ms difference
  it correctly handles product quantity.
* Nlog for logging.
* NUnit for Unit tests.

## Setup

Change configuration options to use selected sql database.  
Change AdminUserOptions to desired values. Admin user will be generated if MigrateDatabase is true and no other users are already present in the database.

**Database must support full-text search, otherwise search by product name won't work.**

## Basic flow

**All endpoints and their summaries can be found in swagger.**

**[Post] api/Identity/Register** User registers, after registration gets auth token.  
**[Get] api/ShoppingCart** Gets cart this assigns a new cart to user  
**[Get] api/Products** gets product list.  
**[Patch] api/ShoppingCart** updates cart with selected product and quantity.  
**[Post] api/Orders** creates new order, fills address information (if user has already created an address he can select addressId instead of filling address information), phone, comment.  

**Database must support full-text search, otherwise search by product name won't work.**

## Additional information

* DbSchema:  
![DB schema image](https://github.com/Pilkas-git/OkredoTask/blob/master/DbSchema.svg)
* Application targets .NET 6
