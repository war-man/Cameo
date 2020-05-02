/*********************************************************************************

	Version: 1.0

**********************************************************************************/

(function($) {
    'use strict';

    // Global State Object
    var state = {};
    window.state = state;
    var windows = $(window);
    var screenSize = windows.width();
    var sticky = $('.header-sticky');
    var $html = $('html');
    var $body = $('body');

    windows.on('scroll', function() {
        var scroll = windows.scrollTop();
        var headerHeight = sticky.height();

        if (screenSize >= 320) {
            if (scroll < headerHeight) {
                sticky.removeClass('is-sticky');
            } else {
                sticky.addClass('is-sticky');
            }
        }

    });

    /*==================================
    	02. Counter Up
    ===================================*/


    $('.count').counterUp({
        delay: 10,
        time: 1000
    });


    /*======================= 
    	01. Wow Active 
    ======================*/

    new WOW().init();


    /*==================================
    	02. Bg Color
    ===================================*/


    var $bgcolor = $('.bg-color');
    $bgcolor.each(function() {
        var $this = $(this),
            $color = $this.data('bg-color');
        $this.css('background-color', $color);
    });



    /*=============================
    	Click Shopping Cart 
    ================================*/

    function ClcickCart() {
        var body = 'body';
        $('.cart-trigger').on('click', function(e) {
            e.preventDefault(),
                $(body).addClass('open-cart-aside');
        })
        $('.btn-close-cart').on('click', function(e) {
            e.preventDefault(),
                $(body).removeClass('open-cart-aside');
        })
        $('.search-flyoveray').on('click', function(e) {
            e.preventDefault(),
                $(body).removeClass('open-cart-aside');
        })
    }
    ClcickCart();



    /*=============================
    	Demo Option
    ================================*/
    var $demoOption = $('.more_demo_aside');
    $('.quick-option').on('click', function(e) {
        e.preventDefault(),
            function() {
                $demoOption.toggleClass('open')
            }()
    });




    /*=============================
    	Search Option
    ================================*/

    function searchoption() {
        $('.search-trigger').on('click', function(e) {
            e.preventDefault(),
                $('.search-flyoverlay-area').addClass('is-visible');
        })
        $('.btn-close-search').on('click', function(e) {
            e.preventDefault(),
                $(this).parent('.search-flyoverlay-area').removeClass('is-visible')
        })
    }
    searchoption();



    /*=============================
    	Hzmbargur Option
    ================================*/

    function hamburgerOption(params) {
        $('.hamburger-trigger').on('click', function(e) {
            e.preventDefault(),
                $('.hamburger-area').addClass('is-visible');
            $(this).addClass('open');

            $(".menu-overlay").addClass('active');

        })
        $('.btn-close-search').on('click', function(e) {
            e.preventDefault(),
                $(this).parent('.hamburger-area').removeClass('is-visible');
            $(".menu-overlay").removeClass('active');
            $('.hamburger-trigger').removeClass('open');
            $('.sub-menu').slideUp('100');
            $('.lavel--3').slideUp('100');
            $('.responsive-manu > li > a').removeClass('is-visiable')
            $('.has-label--3 a').removeClass('is-visiable')
        })
    }
    hamburgerOption();

    function responsiveMenu() {
        $('.responsive-manu > li > a').on('click', function(e) {
            e.preventDefault(),
                $(this).siblings('.sub-menu').slideToggle('400');
            $(this).toggleClass('is-visiable').siblings('.sub-menu').toggleClass('active');
            $('.lavel--3').slideUp('400');
            $('.has-label--3 a').removeClass('is-visiable')
        });
        $('.has-label--3 > a').on('click', function(e) {
            e.preventDefault(),
                $(this).siblings('.lavel--3').slideToggle('400');
            $(this).toggleClass('is-visiable').siblings('.sub-menu').toggleClass('active');
        });
    }
    responsiveMenu();


    /*=============================
        Header Mini Sidebar 
    =============================*/
    $('.vertical-toggle-trigger').on('click', function(e) {
        e.preventDefault(),
            $('.minisidebar__menu').toggleClass('is_visible');
        $(this).toggleClass('is_visible');
    })


    /*======================
        Popup Menu 
    =======================*/

    function popupHeader() {
        $('.popup-trigger').on('click', function(e) {
            e.preventDefault(),
                $('.popup-fly-over-wrapper').addClass('is-visiable')
        })

        $('.close_btn').on('click', function(e) {
            e.preventDefault(),
                $('.popup-fly-over-wrapper').removeClass('is-visiable');
            $('.drdropdown > a').removeClass('is-visiable');
            $('.drlabel2').slideUp('400');
        })
    }
    popupHeader();

    /*===============================
        One page nav active        
    =================================*/

    var top_offset = $('.navigation-menu--onepage').height() - 60;
    $('.navigation-menu--onepage, .target-button').onePageNav({
        currentClass: 'active',
        scrollOffset: top_offset,
    });

    var top_offset_mobile = $('.header-area').height();
    $('.offcanvas-navigation--onepage').onePageNav({
        currentClass: 'active',
        scrollOffset: top_offset_mobile,
    });


    $('.target-button a[href*="#"]:not([href="#"])').click(function() {
        if (location.pathname.replace(/^\//, '') == this.pathname.replace(/^\//, '') && location.hostname == this.hostname) {
            var target = $(this.hash);
            target = target.length ? target : $('[name=' + this.hash.slice(1) + ']');
            if (target.length) {
                $('html, body').animate({
                    scrollTop: target.offset().top - 80
                }, 1000);
                return false;
            }
        }
    });



    /*-- 
        Golobal Click Menu 
    --------------------------------------------------*/
    //Variables
    var $offCanvasNav = $('.mainmenu'),
        $offCanvasNavSubMenu = $offCanvasNav.find('.drlabel2');

    //Add Toggle Button With Off Canvas Sub Menu
    $offCanvasNavSubMenu.parent().prepend('<span class="menu-expand"><i></i></span>');

    //Close Off Canvas Sub Menu
    $offCanvasNavSubMenu.slideUp();

    //Category Sub Menu Toggle
    $offCanvasNav.on('click', 'li a, li .menu-expand', function(e) {
        var $this = $(this);
        if (($this.parent().attr('class').match(/\b(drdropdown|has-children)\b/)) && ($this.attr('href') === '#' || $this.hasClass('menu-expand'))) {
            e.preventDefault();
            if ($this.siblings('ul:visible').length) {
                $this.parent('li').removeClass('active');
                $this.siblings('ul').slideUp();
            } else {
                $this.parent('li').addClass('active');
                $this.closest('li').siblings('li').removeClass('active').find('li').removeClass('active');
                $this.closest('li').siblings('li').find('ul:visible').slideUp();
                $this.siblings('ul').slideDown();
            }
        }
    });

    /*===========================================
        Submenu viewport position     
    =============================================*/

    if ($(".label-1").find('.drop-item').length) {
        var elm = $(".label-1").find('.drop-item');

        elm.each(function() {
            var off = $(this).offset();
            var l = off.left;
            var w = $(this).width();
            var docH = windows.height();
            var docW = windows.width() - 10;
            var isEntirelyVisible = (l + w <= docW);

            if (!isEntirelyVisible) {
                $(this).addClass('left');
            }
        });
    }


    /*=========================== 
        Slick Activation
    ============================*/
    // Check if element exists
    $.fn.elExists = function() {
        return this.length > 0;
    };
    // Variables
    var $html = $('html'),
        $elementCarousel = $('.draven-element-carousel');

    if ($elementCarousel.elExists()) {
        var slickInstances = [];
        $elementCarousel.each(function(index, element) {
            var $this = $(this);
            // Carousel Options
            var $options = typeof $this.data('slick-options') !== 'undefined' ? $this.data('slick-options') : '';
            var $spaceBetween = $options.spaceBetween ? parseInt($options.spaceBetween) : 0,
                $spaceBetween_xl = $options.spaceBetween_xl ? parseInt($options.spaceBetween_xl) : 0,
                $isCustomArrow = $options.isCustomArrow ? $options.isCustomArrow : false,
                $customPrev = $isCustomArrow === true ? ($options.customPrev ? $options.customPrev : '') : '',
                $customNext = $isCustomArrow === true ? ($options.customNext ? $options.customNext : '') : '',
                $vertical = $options.vertical ? $options.vertical : false,
                $focusOnSelect = $options.focusOnSelect ? $options.focusOnSelect : false,
                $asNavFor = $options.asNavFor ? $options.asNavFor : '',
                $fade = $options.fade ? $options.fade : false,
                $autoplay = $options.autoplay ? $options.autoplay : false,
                $autoplaySpeed = $options.autoplaySpeed ? $options.autoplaySpeed : 5000,
                $swipe = $options.swipe ? $options.swipe : false,
                $adaptiveHeight = $options.adaptiveHeight ? $options.adaptiveHeight : false,

                $verticalSwiping = $options.verticalSwiping ? $options.verticalSwiping : false,
                $mouseWheel = $options.mouseWheel ? $options.mouseWheel : false,
                $mouseWheelVertical = $options.mouseWheelVertical ? $options.mouseWheelVertical : false,

                $arrows = $options.arrows ? $options.arrows : false,
                $dots = $options.dots ? $options.dots : false,
                $infinite = $options.infinite ? $options.infinite : false,
                $centerMode = $options.centerMode ? $options.centerMode : false,
                $centerPadding = $options.centerPadding ? $options.centerPadding : '',
                $speed = $options.speed ? parseInt($options.speed) : 1000,
                $prevArrow = $arrows === true ? ($options.prevArrow ? '<span class="' + $options.prevArrow.buttonClass + '"><i class="' + $options.prevArrow.iconClass + '"></i></span>' : '<button class="slick-prev">previous</span>') : '',
                $nextArrow = $arrows === true ? ($options.nextArrow ? '<span class="' + $options.nextArrow.buttonClass + '"><i class="' + $options.nextArrow.iconClass + '"></i></span>' : '<button class="slick-next">next</span>') : '',
                $slidesToShow = $options.slidesToShow ? parseInt($options.slidesToShow, 10) : 1,
                $slidesToScroll = $options.slidesToScroll ? parseInt($options.slidesToScroll, 10) : 1,
                $appendDots = $options.appendDots ? $options.appendDots : $this;

            /*Responsive Variable, Array & Loops*/
            var $responsiveSetting = typeof $this.data('slick-responsive') !== 'undefined' ? $this.data('slick-responsive') : '',
                $responsiveSettingLength = $responsiveSetting.length,
                $responsiveArray = [];
            for (var i = 0; i < $responsiveSettingLength; i++) {
                $responsiveArray[i] = $responsiveSetting[i];

            }

            // Adding Class to instances
            $this.addClass('slick-carousel-' + index);
            $this.parent().find('.slick-dots').addClass('dots-' + index);
            $this.parent().find('.slick-btn').addClass('btn-' + index);

            if ($spaceBetween != 0) {
                $this.addClass('slick-gutter-' + $spaceBetween);
            }
            if ($spaceBetween_xl != 0) {
                $this.addClass('slick-gutter-xl-' + $spaceBetween_xl);
            }

            if ($mouseWheel) {
                $this.on('wheel', (function(e) {
                    e.preventDefault();

                    if (e.originalEvent.deltaY < 0) {
                        $(this).slick('slickNext');
                    } else {
                        $(this).slick('slickPrev');
                    }
                }));
            }



            if ($mouseWheelVertical) {
                $this.on('wheel', function(e) {
                    e.preventDefault();
                    const delta = e.originalEvent.deltaY
                    if (delta > 0) {
                        $this.slick('slickPrev')
                    } else {
                        $this.slick('slickNext')
                    }
                });
            }


            $this.slick({
                slidesToShow: $slidesToShow,
                slidesToScroll: $slidesToScroll,
                asNavFor: $asNavFor,
                autoplay: $autoplay,
                autoplaySpeed: $autoplaySpeed,
                speed: $speed,
                infinite: $infinite,
                arrows: $arrows,
                dots: $dots,
                vertical: $vertical,
                focusOnSelect: $focusOnSelect,
                centerMode: $centerMode,
                centerPadding: $centerPadding,
                fade: $fade,
                adaptiveHeight: $adaptiveHeight,
                verticalSwiping: $verticalSwiping,
                prevArrow: $prevArrow,
                nextArrow: $nextArrow,
                appendDots: $appendDots,
                responsive: $responsiveArray
            });

            if ($isCustomArrow === true) {
                $($customPrev).on('click', function() {
                    $this.slick('slickPrev');
                });
                $($customNext).on('click', function() {
                    $this.slick('slickNext');
                });
            }
        });

        // Updating the sliders in tab
        $('a[data-toggle="tab"]').on('shown.bs.tab', function(e) {
            $elementCarousel.slick('setPosition');
        });
    }

    /*=============================
        Shop filter active 
    ============================= */
    $('.shop-filter-active').on('click', function(e) {
        e.preventDefault();
        $('.product-filter-wrapper').slideToggle();
    })

    /* =============================
        Tilt Hover Animation
    =================================*/

    $('.paralax-image').tilt({
        max: 12,
        speed: 1e3,
        easing: "cubic-bezier(.03,.98,.52,.99)",
        transition: !1,
        perspective: 1e3,
        scale: 1
    });


    /* ========================
         Preloadder   
    ===========================*/

    $(window).load(function() {
        setTimeout(function() {
            $('body').addClass('loaded');
        }, 1000);

    })


    /* ========================
        Custom Counter   
    ===========================*/

    $(window).on('load', function() {
        function customcounter() {

            var comma_separator_number_step = $.animateNumber.numberStepFactories.separator(' ');
            $('.lines').each(function() {
                var $this = $(this);
                var tcount = $this.data("count");
                $this.animateNumber({
                        number: tcount,
                        easing: 'easeInQuad',
                        "font-size": "40px",
                        numberStep: comma_separator_number_step
                    },
                    1000);
            });
        }
        customcounter();
    });



    /*=========================== 
        Youtub Popup 
    ============================*/

    $('.play__btn').yu2fvl();

    /*=====================================
        Portfolio Masonry Activation
    =========================================*/

    $(window).load(function() {
        $('.bk-masonary-wrapper').imagesLoaded(function() {
            // filter items on button click
            $('.messonry-button , .mesonary-button-active').on('click', 'button', function() {
                var filterValue = $(this).attr('data-filter');
                $(this).siblings('.is-checked').removeClass('is-checked');
                $(this).addClass('is-checked');
                $grid.isotope({
                    filter: filterValue
                });
            });
            // init Isotope
            var $grid = $('.mesonry-list').isotope({
                percentPosition: true,
                transitionDuration: '0.7s',
                layoutMode: 'masonry',
                masonry: {
                    columnWidth: '.resizer',
                }
            });
        });

    })


    $(window).load(function() {
        $('.bk-masonary-wrapper').imagesLoaded(function() {
            // filter items on button click
            $('.messonry-button , .mesonary-button-active').on('click', 'button', function() {
                var filterValue = $(this).attr('data-filter');
                $(this).siblings('.is-checked').removeClass('is-checked');
                $(this).addClass('is-checked');
                $grid.isotope({
                    filter: filterValue
                });
            });
            // init Isotope
            var $grid = $('.mesonry-list2').isotope({
                percentPosition: true,
                transitionDuration: '0.7s',
                layoutMode: 'masonry',
                masonry: {
                    columnWidth: '.resizer',
                }
            });
        });
    })

    /*============================== 
        Countdown
    ===============================*/

    $('[data-countdown]').each(function() {
        var $this = $(this),
            finalDate = $(this).data('countdown');
        $this.countdown(finalDate, function(event) {
            $this.html(event.strftime('<span class="ht-count days"><span class="count-inner"><span class="time-count">%-D</span> <p>Days</p></span></span> <span class="ht-count hour"><span class="count-inner"><span class="time-count">%-H</span> <p>Hours</p></span></span> <span class="ht-count minutes"><span class="count-inner"><span class="time-count">%M</span> <p>Minutes</p></span></span> <span class="ht-count second"><span class="count-inner"><span class="time-count">%S</span> <p>Seconds</p></span></span>'));
        });
    });

    /*====================================
        All Animation For Fade Up 
    =======================================*/

    $(window).on('load', function() {
        function allAnimation() {
            $('.move-up').css('opacity', 0);
            $('.move-up').waypoint(function() {
                $('.move-up').addClass('animate');
            }, {
                offset: '90%'
            });
        }
        allAnimation();
    })


    /*=============================
        Radial Progress 02 
    ==============================*/

    $('.radial-progress-single').waypoint(function() {
        $('.radial-progress').easyPieChart({
            lineWidth: 8,
            scaleLength: 0,
            rotate: -45,
            trackColor: false,
            lineCap: 'square',
            size: 200
        })

    }, {
        triggerOnce: true,
        offset: 'bottom-in-view'
    });


    /*===========================
        Tilt Hover Effects
    ==============================*/

    (function() {
        var tiltSettings = [{},
            {
                movement: {
                    imgWrapper: {
                        rotation: {
                            x: -5,
                            y: 10,
                            z: 0
                        },
                        reverseAnimation: {
                            duration: 50,
                            easing: 'easeOutQuad'
                        }
                    },
                    caption: {
                        translation: {
                            x: 20,
                            y: 20,
                            z: 0
                        },
                        reverseAnimation: {
                            duration: 200,
                            easing: 'easeOutQuad'
                        }
                    },
                    overlay: {
                        translation: {
                            x: 5,
                            y: -5,
                            z: 0
                        },
                        rotation: {
                            x: 0,
                            y: 0,
                            z: 6
                        },
                        reverseAnimation: {
                            duration: 1000,
                            easing: 'easeOutQuad'
                        }
                    },
                    shine: {
                        translation: {
                            x: 50,
                            y: 50,
                            z: 0
                        },
                        reverseAnimation: {
                            duration: 50,
                            easing: 'easeOutQuad'
                        }
                    }
                }
            }
        ];

        function init() {
            var idx = 0;
            [].slice.call(document.querySelectorAll('.tilter')).forEach(function(el, pos) {
                idx = pos % 2 === 0 ? idx + 1 : idx;
                new TiltFx(el, tiltSettings[idx - 1]);
            });
        }

        // Preload all images.
        $('.tilter').imagesLoaded(function() {
            init();
        });

    })();


    $('.portfolio-big-thumbnail').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: false,
        fade: true,
        asNavFor: '.portfolio-small-thumbnail'
    });

    $('.portfolio-small-thumbnail').slick({
        slidesToShow: 6,
        slidesToScroll: 1,
        asNavFor: '.portfolio-big-thumbnail',
        dots: false,
        focusOnSelect: true,
        responsive: [{
                breakpoint: 1600,
                settings: {
                    slidesToShow: 5,
                }
            },
            {
                breakpoint: 1199,
                settings: {
                    slidesToShow: 4,
                }
            },
            {
                breakpoint: 991,
                settings: {
                    slidesToShow: 4,
                }
            },
            {
                breakpoint: 767,
                settings: {
                    slidesToShow: 3,
                }
            },
            {
                breakpoint: 479,
                settings: {
                    slidesToShow: 2,
                }
            }
        ]
    });


    /*============================
        Quick View Modal 
    ==============================*/
    $('.quickview').on('click', function(e) {
        e.preventDefault();
        $('.quick-view-modal').toggleClass('is-visible');
    });

    $('.close-quickview-modal').on('click', function() {
        $('.quick-view-modal').removeClass('is-visible');
    });





    /*=====================================
        Product Details Images 
    ======================================*/

    $('.quickview-images').slick({
        slidesToShow: 1,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 5000,
        dots: false,
        infinite: true,
        centerMode: true,
        centerPadding: 0,
        prevArrow: '<span class="slider-navigation slider-navigation-prev"><i class="fa fa-caret-left"></i></span>',
        nextArrow: '<span class="slider-navigation slider-navigation-next"><i class="fa fa-caret-right"></i></span>',
        asNavFor: '.quickview-small-images'
    });

    $('.quickview-small-images').slick({
        slidesToShow: 3,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 5000,
        dots: false,
        infinite: true,
        focusOnSelect: true,
        centerMode: true,
        centerPadding: 0,
        prevArrow: '<span class="slider-navigation slider-navigation-prev"><i class="fa fa-caret-left"></i></span>',
        nextArrow: '<span class="slider-navigation slider-navigation-next"><i class="fa fa-caret-right"></i></span>',
        asNavFor: '.quickview-images'
    });

    /*============================== 
        Scroll Up Activation
    ================================*/
    $.scrollUp({
        scrollText: '<i class="fa fa-angle-up"></i>',
        easingType: 'linear',
        scrollSpeed: 900,
        animation: 'slide'
    });

    /*=================================
        Photo Slider Ativation 
    ==================================*/
    $('.photo4-activation').slick({
        autoplay: true,
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: true,
        fade: true,
        prevArrow: '<span class="photo-navigation photo-navigation-prev"><i class="fa fa-angle-left"></i></span>',
        nextArrow: '<span class="photo-navigation photo-navigation-next"><i class="fa fa-angle-right"></i></span>',
        appendArrows: '.slick-controls'
    });


    /*========================
        Photo Slider Ativation 
    ==========================*/
    $('.photo3-activation').slick({
        autoplay: true,
        infinite: true,
        slidesToShow: 1,
        slidesToScroll: 1,
        arrows: true,
        fade: true,
        prevArrow: '<span class="photo-navigation photo-navigation-prev"><i class="fa fa-angle-left"></i></span>',
        nextArrow: '<span class="photo-navigation photo-navigation-next"><i class="fa fa-angle-right"></i></span>',
        appendArrows: '.slick-controls'
    });



    $('.photo4-play').on('click', function() {
        $('.photo4-activation').slick('slickPlay');
    });

    $('.photo4-pause').on('click', function() {
        $('.photo4-activation').slick('slickPause');
    });

    $('.photo4-play').on('click', function(e) {
        e.preventDefault(),
            $(this).addClass('active');
        $('.photo4-pause').removeClass('active');
    })

    $('.photo4-pause').on('click', function(e) {
        e.preventDefault(),
            $(this).addClass('active');
        $('.photo4-play').removeClass('active');
    })

    /*==============================
        Price Slider Active
    ================================*/

    $('#slider-range').slider({
        range: true,
        min: 10,
        max: 500,
        values: [110, 400],
        slide: function(event, ui) {
            $('#amount').val('$' + ui.values[0] + ' - $' + ui.values[1]);
        }
    });
    $('#amount').val('$' + $('#slider-range').slider('values', 0) +
        " - $" + $('#slider-range').slider('values', 1));


    /*================================
       Quantity
    =================================*/

    $('.pro-qty').prepend('<span class="dec qtybtn">-</span>');
    $('.pro-qty').append('<span class="inc qtybtn">+</span>');
    $('.qtybtn').on('click', function() {
        var $button = $(this);
        var oldValue = $button.parent().find('input').val();
        if ($button.hasClass('inc')) {
            var newVal = parseFloat(oldValue) + 1;
        } else {
            // Don't allow decrementing below zero
            if (oldValue > 0) {
                var newVal = parseFloat(oldValue) - 1;
            } else {
                newVal = 0;
            }
        }
        $button.parent().find('input').val(newVal);
    });


    /*================================== 
        Shipping Form Toggle
    ====================================*/

    $('[data-shipping]').on('click', function() {
        if ($('[data-shipping]:checked').length > 0) {
            $('#shipping-form').slideDown();
        } else {
            $('#shipping-form').slideUp();
        }
    })

    /*==================================
        Payment Method Select
    ====================================*/
    $('[name="payment-method"]').on('click', function() {
        var $value = $(this).attr('value');
        $('.single-method p').slideUp();
        $('[data-method="' + $value + '"]').slideDown();
    })


    // Code Goes Here


    /*--
        Custom Scrollbar (Perfect Scrollbar)
    -----------------------------------*/


    $('.custom-scroll').each(function() {
        var ps = new PerfectScrollbar($(this)[0]);
    });

    // tooltip
    $('[data-toggle="tooltip"]').tooltip()

})(jQuery);