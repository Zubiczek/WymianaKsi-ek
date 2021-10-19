// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('.offerwindow').hover(function () {
	$(this).css("transform", "scale(1.2)");
}, function () {
	$(this).css("transform", "scale(1.0)");
});
$(function () {
	var nickname = $('.user-nickname').text();
	var myprofile = 'Mój profil';
	$('.user').hover(function () {
		$('.user-nickname').hide().text(myprofile).fadeIn("slow");
	}, function () {
		$('.user-nickname').hide().text(nickname).fadeIn("slow");
	});
});