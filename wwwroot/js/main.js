$(function () {
    $(window).on("scroll", function () {
        if ($(window).scrollTop() > 1) {
            $(".logo").addClass("small-width");
            $(".logo img").addClass("small-height");

            $(".logo").addClass("transition-class");
            $(".logo img").addClass("transition-class");
        }  else {
            $(".logo").removeClass("small-width");
            $(".logo img").removeClass("small-height");

            $(".logo").addClass("transition-class");
            $(".logo img").addClass("transition-class");
        }
    });
});

// Toggle hide bar

var menu_btn = document.getElementById("menuBtn");
var menu = document.getElementById("menuTop");
// var redes = document.getElementById("logoRedesSociais");

menu_btn.onclick = function() {
    menu.classList.toggle("hide");
    // redes.classList.toggle("hide");
}

// Alerts - Social medias accounts

function facebook_click()
{
    alert("Agora você seria redirecionado para a página do Facebook do Carfel.");
}

function twitter_click() {
    alert("Agora você seria redirecionado para a página do Twitter do Carfel.");
}

function instagram_click() {
    alert("Agora você seria redirecionado para a página do Instagram do Carfel.");
}

// Select all links with hashes
$('a[href*="#"]')
  // Remove links that don't actually link to anything
  .not('[href="#"]')
  .not('[href="#0"]')
  .click(function(event) {
    // On-page links
    if (
      location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') 
      && 
      location.hostname == this.hostname
    ) {
      // Figure out element to scroll to
      var target = $(this.hash);
      target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
      // Does a scroll target exist?
      if (target.length) {
        // Only prevent default if animation is actually gonna happen
        event.preventDefault();
        $('html, body').animate({
          scrollTop: target.offset().top
        }, 1000, function() {
          // Callback after animation
          // Must change focus!
          var $target = $(target);
          $target.focus();
          if ($target.is(":focus")) { // Checking if the target was focused
            return false;
          } else {
            $target.attr('tabindex','-1'); // Adding tabindex for elements not focusable
            $target.focus(); // Set focus again
          };
        });
      }
    }
  });