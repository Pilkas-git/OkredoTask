# Project

Online shop implementation for Okredo task.

Includes:
* Category caching,
* Full-Text search for product names.
* Two sepparate order strategies for registered/unregistered users. If user is not logged in, user is identified by cartCookie value from request header.
* Optimistic concurrency for product quantity. If multiple users manage to order same product with ms difference
  it correctly handles product quantity.

## Setup

Change configuration options to use selected sql database.  
Change AdminUserOptions to desired values. Admin user will be generated if MigrateDatabase is true and no other users are already present in the database.

**Database must support full-text search, otherwise search by product name won't work.**

## Basic flow registered

User registers, after registration gets auth token.
Gets cart
Get product list
Patches cart with selected product
Posts new order, fills address information, phone

## Basic flow UnRegistered

Gets cart
Get product list
Patches cart with selected product
Posts order, fills address information, phone, email

**Database must support full-text search, otherwise search by product name won't work.**

## Additional information

* DbSchema:  
![DB schema image](https://github.com/Pilkas-git/OkredoTask/blob/master/DbSchema.svg)
* Application targets .NET 6
