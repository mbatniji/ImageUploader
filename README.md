# ImageUploader

This package is for uploading images or files:
To Upload Image:
var Path = "wwwroot\Attach";
var savedFileName=await Images.Upload(Path , file);
To Upload File:
var Path = "wwwroot\Attach";
var ZiptheFile =true; //to save the file as zip file.
var savedFileName= await Files.Upload(Path , file, ZiptheFile );
