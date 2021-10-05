$('.offerwindow').hover(function () {
		$(this).css("transform", "scale(1.2)");
		}, function () {
		$(this).css("transform", "scale(1.0)");
		});

$( function() {
var nickname = $('.user-nickname').text();
var myprofile = 'Mój profil';
$('.user').hover(function() {
	$('.user-nickname').hide().text(myprofile).fadeIn("slow");
}, function() {
	$('.user-nickname').hide().text(nickname).fadeIn("slow");
});
});
