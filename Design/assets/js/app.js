var swiper = new Swiper(".mySwiper", {
  spaceBetween: 10,
  slidesPerView: 6,
  freeMode: true,
  watchSlidesProgress: true,
  
});
var swiper2 = new Swiper(".mySwiper2", {
  spaceBetween: 10,
  loop: true,
  speed: 800,
  //effect: 'cards',
  autoplay: {
      delay: 4000,
      disableOnInteraction: false
  },

  thumbs: {
      swiper: swiper,
  },
});

// product Gallery and Zoom

$('.gallery-parent').each(function () {
  // We finding any "gallery-parent" and find child with class "gallery-top" and "gallery-thumbs" for multiple using plugin
  let thumbs = $(this).children('.gallery-thumbs'),
      top = $(this).children('.gallery-top');

  // activation carousel plugin
  let galleryThumbs = new Swiper(thumbs, {
      spaceBetween: 5,
      freeMode: true,
      watchSlidesVisibility: true,
      watchSlidesProgress: true,
      breakpoints: {
          0: {
              slidesPerView: 3,
          },
          992: {
              slidesPerView: 4,
          },
      },
  });
  let galleryTop = new Swiper(top, {
      spaceBetween: 10,
      navigation: {
          nextEl: '.swiper-button-next',
          prevEl: '.swiper-button-prev',
      },
      thumbs: {
          swiper: galleryThumbs,
      },
  });

  // change carousel item height
  // gallery-top
  let productCarouselTopWidth = top.outerWidth();
  top.css('height', productCarouselTopWidth);

  // gallery-thumbs
  let productCarouselThumbsItemWith = thumbs.find('.swiper-slide').outerWidth();
  thumbs.css('height', productCarouselThumbsItemWith);
});

// activation zoom plugin
let $easyzoom = $('.easyzoom').easyZoom();

//smooth scrolling 
const links = document.querySelectorAll(".navbar .navbar-nav .nav-item a");

for (const link of links) {
  if (link.getAttribute("href").includes("#")) {
      link.addEventListener("click", clickHandler);
  }
}

function clickHandler(e) {
  e.preventDefault();
  const href = this.getAttribute("href");
  const offsetTop = document.querySelector(href).offsetTop;

  scroll({
      top: offsetTop,
      behavior: "smooth"
  });
}

document.addEventListener("DOMContentLoaded", function () {
  window.addEventListener('scroll', function () {
      if (window.scrollY > 50) {
          document.getElementById('navbar_top').classList.add('fixed-top');
          // add padding top to show content behind navbar
          navbar_height = document.querySelector('.navbar').offsetHeight;
          document.body.style.paddingTop = navbar_height;
      } else {
          document.getElementById('navbar_top').classList.remove('fixed-top');
          // remove padding top from body
          document.body.style.paddingTop = '0';
      }
  });
});