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

$('#user-opinion1').hover(function () {
	$(this).css("color", "orange");
}, function () {
	$(this).css("color", "black");
});

$('#user-opinion2').hover(function () {
	for (let i = 1; i <= 2; i++) {
		let divid = "#user-opinion" + i;
		$(divid).css("color", "orange");
	}
}, function () {
	for (let i = 1; i <= 2; i++) {
		let divid = "#user-opinion" + i;
		$(divid).css("color", "black");
	}
});

$('#user-opinion3').hover(function () {
	for (let i = 1; i <= 3; i++) {
		let divid = "#user-opinion" + i;
		$(divid).css("color", "orange");
	}
}, function () {
	for (let i = 1; i <= 3; i++) {
		let divid = "#user-opinion" + i;
		$(divid).css("color", "black");
	}
});

$('#user-opinion4').hover(function () {
	for (let i = 1; i <= 4; i++) {
		let divid = "#user-opinion" + i;
		$(divid).css("color", "orange");
	}
}, function () {
	for (let i = 1; i <= 4; i++) {
		let divid = "#user-opinion" + i;
		$(divid).css("color", "black");
	}
});

$('#user-opinion5').hover(function () {
	for (let i = 1; i <= 5; i++) {
		let divid = "#user-opinion" + i;
		$(divid).css("color", "orange");
	}
}, function () {
	for (let i = 1; i <= 5; i++) {
		let divid = "#user-opinion" + i;
		$(divid).css("color", "black");
	}
});
