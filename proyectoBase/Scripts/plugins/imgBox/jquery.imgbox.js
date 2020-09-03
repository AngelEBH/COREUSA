/* FUNCIONES PARA VISUALIZAR DOCUMENTACION */
var angle = 0;

!function ($) {

	var defaults = {
		//soportar zoom
		zoom: true,
		// permitir arrastre
		drag: true,
		// La relación máxima después de que se amplía la imagen, el valor predeterminado es la visualización de la imagen original, este valor no excederá el ancho de la boca de mina
		scale: 1,
	};

	$.fn.imgbox = function(options){

		options = $.extend({}, defaults, options);

		$(this).css({
			cursor: "zoom-in"
		}).parents("a").attr("href", "javascript:void(0);").click(function(){
			return false;
		}).css({
			cursor: "default",
		});

		$(this).click(function(){

			var src = $(this).attr("src");			

			var mask = $('<div data-pop-layer="1" style=""></div>').css({
				position: "fixed",
				left: "0px",
				right: "0px",
				top: "0px",
				bottom: "0px",
				height: "100%",
				width: "100%",
				zIndex: "9999999",
				backgroundColor: "none",
				opacity: 0.8,
				userSelect: "none"
			});

			var image = $('<img src="' + $(this).attr("src") + '" >').css({
				position: "fixed",
				top: "0px",
				zIndex: "10000000",
				display: "none"
			});			

			function rotar() {
				angle += 90;
				$(image).rotate(angle);
            }

			var rotatebtn = $('<button class="check-original-image btn btn-hover"><i class="mdi mdi-rotate-right"></i></button>').on('click', function () {				
				rotar();
			});

			var closebtn = $('<button class="close-original-image" href="javascript:void(0);"><i class="mdi mdi mdi-close-circle-outline"></i></button>');


			var width = $(image).prop("width");
			var height = $(image).prop("height");

			var scale = width / height;

			//$('html').append(closebtn).append(image).append(rotatebtn).append(mask).css({
			//	overflow: 'hidden'
			//});


			$('html').append(image).append(rotatebtn).append(mask).css({
				overflow: 'hidden'
			});

			$(image).fadeIn("fast");

			$(rotatebtn).click(function(){
				event.stopPropagation();
			});

			var resize_handler  = function(){

				var w = width;
				var h = height;

				var max_width = $(window).width() - 5;
				var max_height = $(window).height();

				w = w * options.scale < max_width ? w * options.scale : max_width;
				h = w / scale;

				if(h > max_height){
					h = max_height;
					w = h * scale;
				}

				$(image).width(w);
				$(image).height(h);

				$(image).css({
					left: ($(window).width() - w) / 2 + "px",
					top: ($(window).height() - h) / 2 + "px"
				});

			};

			var close_handler = function(){

				$(window).off("resize", resize_handler).off("mousewheel", scale_handler).off("DOMMouseScroll", scale_handler);

				$(image).fadeOut("fast", function(){
					$(this).remove();
				});

				$('html').css({
					overflow: "auto"
				});

				//$('body').css({
				//	//overflow: 'auto',
				//	////height: 'auto',
				//	////height: 'calc(100vh - 5px)',
				//	//'overflow-x':'hidden'
				//});

				$(rotatebtn).remove();
				$(closebtn).remove();
				$(mask).remove();

				if(options.drag){
					$(this).remove();
				}else{
					$(window).off("click", close_handler);
				}

				return false;
			};
			//cerrar imagen al darle doble click
			$(image).on('dblclick',close_handler);

			var scale_handler = function(e, delta) {

				var scale = $(image).data("scale");

				if(!scale){
					scale = 1;
				}

				if(e.originalEvent.wheelDelta > 0 || e.originalEvent.detail < 0){
					// Desplazarse hacia arriba
					scale += 0.1;
				}else{
					// Desplazarse hacia abajo
					scale -= 0.1;

					if(scale <= 0.01){
						return;
					}
				}

				$(image).css({
					transform: "scale(" + scale + ")" + " " + "rotate(" + angle + "deg)"
				}).data("scale", scale);

				//$(image).rotate(angle);

				return false;
			};

			// Arrastrar la imagen
			if(options.drag == true){				

				// Estilo del ratón
				$(image).css({
					cursor: "move",
				});

				function drag(obj) {
			        $(obj).mousedown(function(ev) {
			            var ev = ev || event;

			            var disX = ev.clientX - this.offsetLeft;
			            var disY = ev.clientY - this.offsetTop;

			            if (obj.setCapture) {
			                obj.setCapture();
			            }

			            var mousemove = function(ev) {
			                var ev = ev || event;

			                obj.style.left = ev.clientX - disX + 'px';
			                obj.style.top = ev.clientY - disY + 'px';
			            };

			            var mouseup = function() {

			            	$(document).off("mousemove", mousemove).off("mouseup", mouseup);

			                // Liberar captura global releaseCapture();
			                if (obj.releaseCapture) {
			                    obj.releaseCapture();
			                }
			            };

			            $(document).on("mousemove", mousemove);

			            $(document).on("mouseup", mouseup);

			            return false;

			        });

			    }

				drag(image.get(0));

				// Cerrar botón clic evento
				$(closebtn).click(close_handler);
			}else{
				// Estilo del ratón
				$(image).css({
					cursor: "zoom-out",
				});
				// Haga clic en la ventana para cerrar.
				$(window).click(close_handler);
			}

			// Evento de cambio de ventana
			$(window).on("resize", resize_handler).trigger("resize");

			// Zoom de imagen
			if(options.zoom){
				$(window).on("mousewheel DOMMouseScroll", scale_handler);
			}
		});
	};

}(jQuery);
