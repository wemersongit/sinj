/* Responsive Hybrid Menu
* Author: Dynamic Drive at http://www.dynamicdrive.com/
* Visit http://www.dynamicdrive.com/ for full source code
* Updated March 25th 14' for menu links not working bug fix
* Updated March 27th 14': Added "hassub" class to sub menu LIs that contain additional sub menus (ULs)
*/


jQuery.noConflict();
(function($) { 
  $(function() {

  	//Global Animation speed initialization
  	var speeed = 400;

  	//Global Animation Type initialization
  	var type = 'swing';

  	//One time navigation selection for further use
  	var theMenu = $('#hybridmenu');

  	//Mobile specific Menu Item selection
  	var mobilMenuItem = theMenu.find('ul li.mobile-menu-item');

  	//Whole Content selection
  	var wholeContent = $('#wrapper, nav#hybridmenu');

  	//Body Height Calculation
  	var bodyHeight = $('body').outerHeight();

		//Add "hassub" class to LIs with sub menus (additional ULs)
		theMenu.find('ul.sub-menu').parent('li').addClass('hassub')

  	//Claculating menu height to force whole content go down to avoid missing any content
    var theMenuHeight = theMenu.outerHeight();
    theMenu.next().css('marginTop', theMenuHeight + 'px');

	//Mobile Window Size event trigger on scroll Fix
	var width = $(window).width();
	window.theWidth = width;
	//alert('this.width: ' + window.theWidth);

    // Mouse enter function call
    theMenu.find('.parent').on('mouseenter', document, theSlideDown);

    // Mouse leave function call
    theMenu.find('.parent').on('mouseleave', document, theSlideUp);

    //Preparing for Mobile Menu
    theMenu.find('ul.main-menu').clone().appendTo('#mobile-menu');
    mobileMenu = $('#mobile-menu');
    mobileMenu.css('minHeight', bodyHeight + 'px');
    mobileMenu.hide();
    mobileMenu.find('ul.main-menu > li.parent').each(function(){
		var mobileHeading = $(this).find('>a').clone().addClass("mobile-heading");
		mobileHeading.prependTo($(this).find('ul.sub-menu'));
    });
    $('.mobile-heading').prepend('<i class="fa fa-chevron-left" />')
	mobileMenu.find('.mobile-menu-item, .logo').remove();
	mobileMenu.find('li a').append('<i class="fa fa-chevron-right"></i>');
	//Mobile Menu Button Click Handler Call
	mobilMenuItem.on('click', document, mobileItemClicked);

	// Mobile Sub-menu Item retrieved call
	mobileMenu.find('li.parent').on('click', document, mobileSubItemClicked);

	//Mobile Heading Clicked Call
	$('.mobile-heading').on('click', document, mobileHeadingClicked);

	//Window Resize Call
	$(window).resize(windowResized);

    // Mouse enter function - revealing the dropdown
	function theSlideDown(e) {
		if (!mobilMenuItem.is(':visible')) {
			e.stopPropagation();
			$(this).off('mouseleave', document, theSlideUp);
			$(this).find('ul').stop(true, true).slideDown(speeed, function(){
				$(this).off('mouseleave', document, theSlideUp);
			});
		}
	} 

	// Mouse leave function - hiding the dropdown
	function theSlideUp(e) {
		if (!mobilMenuItem.is(':visible')) {
			e.stopPropagation();
			$(this).off('mouseenter', document, theSlideDown);
			$(this).find('ul').stop(true, true).slideUp(speeed, function(){
				$(this).on('mouseenter', document, theSlideDown);
			});
		}
	}

	//Mobile Menu Button Click Handler
	function mobileItemClicked(e) {
		if (mobileMenu.is(':visible')) {
			wholeContent.removeClass('mobile-opened', speeed, type, function(){
				mobileMenu.hide();
			});
		} else {
			mobileMenu.show();
			wholeContent.addClass('mobile-opened', speeed, type);			
		}
		$('.mobile-heading').trigger('click');
	}

	function windowResized() {
		//alert('Inside:this.width: ' + window.theWidth);
		//Using Mobile Fix
		currentWidth = $(window).width();
		//alert('currentwidth: ' + currentWidth);
		if ( currentWidth != window.theWidth) {
			wholeContent.removeClass('mobile-opened', speeed, type, function(){
				mobileMenu.hide();			
			});	
			$('.mobile-heading').trigger('click');
			window.theWidth = currentWidth;
		}
	}				

	//Mobile Sub-menu Item retrieved
	function mobileSubItemClicked(e) {
		$(this).find('ul.sub-menu').addClass('opened', speeed, type);
		if (e.target.tagName != "A")
			return false;
	}

	//Mobile Heading Clicked
	function mobileHeadingClicked(e) {
		$(this).closest('ul.sub-menu').removeClass('opened', speeed, type);
		return false;
	}	

  });
})(jQuery);
