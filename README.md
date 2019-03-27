# Bumptech.Glide.Integration.RecyclerView

![logo](https://raw.githubusercontent.com/xyfoo/Bumptech.Glide.Integration.RecyclerView/master/art/logo.png)

Xamarin.Android implementation of Glide's RecyclerView integration library

[![NuGet](https://img.shields.io/nuget/vpre/Bumptech.Glide.Integration.RecyclerView.svg?label=NuGet)](https://www.nuget.org/packages/Bumptech.Glide.Integration.RecyclerView)

## Introduction

This is a C# implementation of Glide's RecycleView intergration library.


>[Glide](https://bumptech.github.io/glide/) is a fast and efficient image loading library for Android focused on smooth scrolling. Glide offers an easy to use API, a performant and extensible resource decoding pipeline and automatic resource pooling.
>
> ...
>
> The [RecyclerView integration library](https://bumptech.github.io/glide/int/recyclerview.html) makes the RecyclerViewPreloader available in your application. RecyclerViewPreloader can automatically load images just ahead of where a user is scrolling in a RecyclerView.
>
>Combined with the right image size and an effective disk cache strategy, this library can dramatically decrease the number of loading tiles/indicators users see when scrolling through lists of images by ensuring that the images the user is about to reach are already in memory.

## Dependencies

* Xamarin.Android.Glide, v 4.0.0

## Walkthrough

TODO

---

## FAQ

### Why reimplement instead of creating a binding library?

Attempts to create binding to the [4.0.0 library](https://github.com/bumptech/glide/releases/download/v4.0.0/glide-recyclerview-integration-4.0.0.jar) yield this error: ```BINDINGSGENERATOR : warning BG8601: No packages found```... and I have no idea why that happened.

It turns out the sources code is small, and a reimplementation in C# is faster than to figure out why the bindings build failed.

### The Glide version used by Xamarin.Android.Glide is outdated (v4.0.0)

Xamarin.Android.Glide v4.0.0 package is the only one nuget package with full blown bindings. It's probably easier to take a Xamarin official bindings instead of taking another unproven one.

[Discussion on updating Xamarin.Android.Glide](https://github.com/xamarin/XamarinComponents/issues/464)

### Can this works on later version of Glide?

I didn't notice any difference between v4.0-v4.8 (latest checked on 27 Match 2019) of this RecyclerView integration library. 

So, if you can find a newer binding library for Glide, you should be able copy/paste the content in ```RecyclerView.cs``` and it should work.
