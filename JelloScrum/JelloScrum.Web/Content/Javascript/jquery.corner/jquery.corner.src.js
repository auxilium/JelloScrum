// JRC (jquery-round-corners)
// www.meerbox.nl

// Excanvas:

// Copyright 2006 Google Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

if (!document.createElement('canvas').getContext) {

(function() {

  // alias some functions to make (compiled) code shorter
  var m = Math;
  var mr = m.round;
  var ms = m.sin;
  var mc = m.cos;

  // this is used for sub pixel precision
  var Z = 10;
  var Z2 = Z / 2;

  /**
   * This funtion is assigned to the <canvas> elements as element.getContext().
   * @this {HTMLElement}
   * @return {CanvasRenderingContext2D_}
   */
  function getContext() {
    if (this.context_) {
      return this.context_;
    }
    return this.context_ = new CanvasRenderingContext2D_(this);
  }

  var slice = Array.prototype.slice;

  function bind(f, obj, var_args) {
    var a = slice.call(arguments, 2);
    return function() {
      return f.apply(obj, a.concat(slice.call(arguments)));
    };
  }

  var G_vmlCanvasManager_ = {
    init: function(opt_doc) {
      if (/MSIE/.test(navigator.userAgent) && !window.opera) {
        var doc = opt_doc || document;
        // Create a dummy element so that IE will allow canvas elements to be
        // recognized.
        doc.createElement('canvas');
        doc.attachEvent('onreadystatechange', bind(this.init_, this, doc));
      }
    },

    init_: function(doc) {
      // create xmlns
      if (!doc.namespaces['g_vml_']) {
        doc.namespaces.add('g_vml_', 'urn:schemas-microsoft-com:vml');
      }

      // Setup default CSS.  Only add one style sheet per document
      if (!doc.styleSheets['ex_canvas_']) {
        var ss = doc.createStyleSheet();
        ss.owningElement.id = 'ex_canvas_';
        ss.cssText = 'canvas{display:inline-block;overflow:hidden;' +
            // default size is 300x150 in Gecko and Opera
            'text-align:left;width:300px;height:150px}' +
            'g_vml_\\:*{behavior:url(#default#VML)}';
      }

    },

    /**
     * Public initializes a canvas element so that it can be used as canvas
     * element from now on. This is called automatically before the page is
     * loaded but if you are creating elements using createElement you need to
     * make sure this is called on the element.
     * @param {HTMLElement} el The canvas element to initialize.
     * @return {HTMLElement} the element that was created.
     */
    i: function(el) {
      if (!el.getContext) {

        el.getContext = getContext;

        // do not use inline function because that will leak memory
        el.attachEvent('onpropertychange', onPropertyChange);
        el.attachEvent('onresize', onResize);

        var attrs = el.attributes;
        if (attrs.width && attrs.width.specified) {
          // TODO: use runtimeStyle and coordsize
          // el.getContext().setWidth_(attrs.width.nodeValue);
          el.style.width = attrs.width.nodeValue + 'px';
        } else {
          el.width = el.clientWidth;
        }
        if (attrs.height && attrs.height.specified) {
          // TODO: use runtimeStyle and coordsize
          // el.getContext().setHeight_(attrs.height.nodeValue);
          el.style.height = attrs.height.nodeValue + 'px';
        } else {
          el.height = el.clientHeight;
        }
        //el.getContext().setCoordsize_()
      }
      return el;
    }
  };

  function onPropertyChange(e) {
    var el = e.srcElement;

    switch (e.propertyName) {
      case 'width':
        el.style.width = el.attributes.width.nodeValue + 'px';
        el.getContext().clearRect();
        break;
      case 'height':
        el.style.height = el.attributes.height.nodeValue + 'px';
        el.getContext().clearRect();
        break;
    }
  }

  function onResize(e) {
    var el = e.srcElement;
    if (el.firstChild) {
      el.firstChild.style.width =  el.clientWidth + 'px';
      el.firstChild.style.height = el.clientHeight + 'px';
    }
  }

  G_vmlCanvasManager_.init();

  // precompute "00" to "FF"
  var dec2hex = [];
  for (var i = 0; i < 16; i++) {
    for (var j = 0; j < 16; j++) {
      dec2hex[i * 16 + j] = i.toString(16) + j.toString(16);
    }
  }

  function createMatrixIdentity() {
    return [
      [1, 0, 0],
      [0, 1, 0],
      [0, 0, 1]
    ];
  }

  function processStyle(styleString) {
    var str, alpha = 1;

    styleString = String(styleString);
    if (styleString.substring(0, 3) == 'rgb') {
      var start = styleString.indexOf('(', 3);
      var end = styleString.indexOf(')', start + 1);
      var guts = styleString.substring(start + 1, end).split(',');

      str = '#';
      for (var i = 0; i < 3; i++) {
        str += dec2hex[Number(guts[i])];
      }

      if (guts.length == 4 && styleString.substr(3, 1) == 'a') {
        alpha = guts[3];
      }
    } else {
      str = styleString;
    }

    return [str, alpha];
  }

  function processLineCap(lineCap) {
    switch (lineCap) {
      case 'butt':
        return 'flat';
      case 'round':
        return 'round';
      case 'square':
      default:
        return 'square';
    }
  }

  /**
   * This class implements CanvasRenderingContext2D interface as described by
   * the WHATWG.
   * @param {HTMLElement} surfaceElement The element that the 2D context should
   * be associated with
   */
  function CanvasRenderingContext2D_(surfaceElement) {
    this.m_ = createMatrixIdentity();

    this.mStack_ = [];
    this.aStack_ = [];
    this.currentPath_ = [];

    // Canvas context properties
    this.strokeStyle = '#000';
    this.fillStyle = '#000';

    this.lineWidth = 1;
    this.lineJoin = 'miter';
    this.lineCap = 'butt';
    this.miterLimit = Z * 1;
    this.globalAlpha = 1;
    this.canvas = surfaceElement;

    var el = surfaceElement.ownerDocument.createElement('div');
    el.style.width =  surfaceElement.clientWidth + 'px';
    el.style.height = surfaceElement.clientHeight + 'px';
    el.style.overflow = 'hidden';
    el.style.position = 'absolute';
    surfaceElement.appendChild(el);

    this.element_ = el;
    this.arcScaleX_ = 1;
    this.arcScaleY_ = 1;
  }

  var contextPrototype = CanvasRenderingContext2D_.prototype;
  contextPrototype.clearRect = function() {
    this.element_.innerHTML = '';
    this.currentPath_ = [];
  };

  contextPrototype.beginPath = function() {
    // TODO: Branch current matrix so that save/restore has no effect
    //       as per safari docs.
    this.currentPath_ = [];
  };

  contextPrototype.moveTo = function(aX, aY) {
    var p = this.getCoords_(aX, aY);
    this.currentPath_.push({type: 'moveTo', x: p.x, y: p.y});
    this.currentX_ = p.x;
    this.currentY_ = p.y;
  };

  contextPrototype.lineTo = function(aX, aY) {
    var p = this.getCoords_(aX, aY);
    this.currentPath_.push({type: 'lineTo', x: p.x, y: p.y});

    this.currentX_ = p.x;
    this.currentY_ = p.y;
  };

  contextPrototype.bezierCurveTo = function(aCP1x, aCP1y,
                                            aCP2x, aCP2y,
                                            aX, aY) {
    var p = this.getCoords_(aX, aY);
    var cp1 = this.getCoords_(aCP1x, aCP1y);
    var cp2 = this.getCoords_(aCP2x, aCP2y);
    this.currentPath_.push({type: 'bezierCurveTo',
                           cp1x: cp1.x,
                           cp1y: cp1.y,
                           cp2x: cp2.x,
                           cp2y: cp2.y,
                           x: p.x,
                           y: p.y});
    this.currentX_ = p.x;
    this.currentY_ = p.y;
  };

  

  contextPrototype.fillRect = function(aX, aY, aWidth, aHeight) {
    // Will destroy any existing path (same as FF behaviour)
    this.beginPath();
    this.moveTo(aX, aY);
    this.lineTo(aX + aWidth, aY);
    this.lineTo(aX + aWidth, aY + aHeight);
    this.lineTo(aX, aY + aHeight);
    this.closePath();
    this.fill();
    this.currentPath_ = [];
  };

  contextPrototype.createLinearGradient = function(aX0, aY0, aX1, aY1) {
    return new CanvasGradient_('gradient');
  };

  contextPrototype.createRadialGradient = function(aX0, aY0,
                                                   aR0, aX1,
                                                   aY1, aR1) {
    var gradient = new CanvasGradient_('gradientradial');
    gradient.radius1_ = aR0;
    gradient.radius2_ = aR1;
    gradient.focus_.x = aX0;
    gradient.focus_.y = aY0;
    return gradient;
  };

  contextPrototype.stroke = function(aFill) {
    var lineStr = [];
    var lineOpen = false;
    var a = processStyle(aFill ? this.fillStyle : this.strokeStyle);
    var color = a[0];
    var opacity = a[1] * this.globalAlpha;

    var W = 10;
    var H = 10;

    lineStr.push('<g_vml_:shape',
                 ' fillcolor="', color, '"',
                 ' filled="', Boolean(aFill), '"',
                 ' style="position:absolute;width:', W, ';height:', H, ';"',
                 ' coordorigin="0 0" coordsize="', Z * W, ' ', Z * H, '"',
                 ' stroked="', !aFill, '"',
                 ' strokeweight="', this.lineWidth, '"',
                 ' strokecolor="', color, '"',
                 ' path="');

    var newSeq = false;
    var min = {x: null, y: null};
    var max = {x: null, y: null};

    for (var i = 0; i < this.currentPath_.length; i++) {
      var p = this.currentPath_[i];
      var c;

      switch (p.type) {
        case 'moveTo':
          lineStr.push(' m ');
          c = p;
          lineStr.push(mr(p.x), ',', mr(p.y));
          break;
        case 'lineTo':
          lineStr.push(' l ');
          lineStr.push(mr(p.x), ',', mr(p.y));
          break;
        case 'close':
          lineStr.push(' x ');
          p = null;
          break;
        case 'bezierCurveTo':
          lineStr.push(' c ');
          lineStr.push(mr(p.cp1x), ',', mr(p.cp1y), ',',
                       mr(p.cp2x), ',', mr(p.cp2y), ',',
                       mr(p.x), ',', mr(p.y));
          break;
        case 'at':
        case 'wa':
          lineStr.push(' ', p.type, ' ');
          lineStr.push(mr(p.x - this.arcScaleX_ * p.radius), ',',
                       mr(p.y - this.arcScaleY_ * p.radius), ' ',
                       mr(p.x + this.arcScaleX_ * p.radius), ',',
                       mr(p.y + this.arcScaleY_ * p.radius), ' ',
                       mr(p.xStart), ',', mr(p.yStart), ' ',
                       mr(p.xEnd), ',', mr(p.yEnd));
          break;
      }


      // TODO: Following is broken for curves due to
      //       move to proper paths.

      // Figure out dimensions so we can do gradient fills
      // properly
      if (p) {
        if (min.x == null || p.x < min.x) {
          min.x = p.x;
        }
        if (max.x == null || p.x > max.x) {
          max.x = p.x;
        }
        if (min.y == null || p.y < min.y) {
          min.y = p.y;
        }
        if (max.y == null || p.y > max.y) {
          max.y = p.y;
        }
      }
    }
    lineStr.push(' ">');

    if (typeof this.fillStyle == 'object') {
      var focus = {x: '50%', y: '50%'};
      var width = max.x - min.x;
      var height = max.y - min.y;
      var dimension = width > height ? width : height;

      focus.x = mr(this.fillStyle.focus_.x / width * 100 + 50) + '%';
      focus.y = mr(this.fillStyle.focus_.y / height * 100 + 50) + '%';

      var colors = [];

      // inside radius (%)
      if (this.fillStyle.type_ == 'gradientradial') {
        var inside = this.fillStyle.radius1_ / dimension * 100;

        // percentage that outside radius exceeds inside radius
        var expansion = this.fillStyle.radius2_ / dimension * 100 - inside;
      } else {
        var inside = 0;
        var expansion = 100;
      }

      var insidecolor = {offset: null, color: null};
      var outsidecolor = {offset: null, color: null};

      // We need to sort 'colors' by percentage, from 0 > 100 otherwise ie
      // won't interpret it correctly
      this.fillStyle.colors_.sort(function(cs1, cs2) {
        return cs1.offset - cs2.offset;
      });

      for (var i = 0; i < this.fillStyle.colors_.length; i++) {
        var fs = this.fillStyle.colors_[i];

        colors.push(fs.offset * expansion + inside, '% ', fs.color, ',');

        if (fs.offset > insidecolor.offset || insidecolor.offset == null) {
          insidecolor.offset = fs.offset;
          insidecolor.color = fs.color;
        }

        if (fs.offset < outsidecolor.offset || outsidecolor.offset == null) {
          outsidecolor.offset = fs.offset;
          outsidecolor.color = fs.color;
        }
      }
      colors.pop();

      lineStr.push('<g_vml_:fill',
                   ' color="', outsidecolor.color, '"',
                   ' color2="', insidecolor.color, '"',
                   ' type="', this.fillStyle.type_, '"',
                   ' focusposition="', focus.x, ', ', focus.y, '"',
                   ' colors="', colors.join(''), '"',
                   ' opacity="', opacity, '" />');
    } else if (aFill) {
      lineStr.push('<g_vml_:fill color="', color, '" opacity="', opacity,
                   '" />');
    } else {
      var lineWidth = Math.max(this.arcScaleX_, this.arcScaleY_) *
          this.lineWidth;
      lineStr.push(
        '<g_vml_:stroke',
        ' opacity="', opacity, '"',
        ' joinstyle="', this.lineJoin, '"',
        ' miterlimit="', this.miterLimit, '"',
        ' endcap="', processLineCap(this.lineCap), '"',
        ' weight="', lineWidth, 'px"',
        ' color="', color, '" />'
      );
    }

    lineStr.push('</g_vml_:shape>');

    this.element_.insertAdjacentHTML('beforeEnd', lineStr.join(''));
  };

  contextPrototype.fill = function() {
    this.stroke(true);
  };

  contextPrototype.closePath = function() {
    this.currentPath_.push({type: 'close'});
  };

  contextPrototype.getCoords_ = function(aX, aY) {
    return {
      x: Z * (aX * this.m_[0][0] + aY * this.m_[1][0] + this.m_[2][0]) - Z2,
      y: Z * (aX * this.m_[0][1] + aY * this.m_[1][1] + this.m_[2][1]) - Z2
    }
  };

  function CanvasPattern_() {}

  // set up externs
  G_vmlCMjrc = G_vmlCanvasManager_;
  //CanvasRenderingContext2D = CanvasRenderingContext2D_;
  //CanvasGradient = CanvasGradient_;
  //CanvasPattern = CanvasPattern_;

})();

} // if


if (jQuery.browser.msie) {
//	document.execCommand("BackgroundImageCache", false, true);
}

		
(function($){
		
	var isMSIE = $.browser.msie;
	var isltMSIE7 = isMSIE && !window.XMLHttpRequest;
	var isOpera = $.browser.opera;
	var canvasSupport = typeof document.createElement('canvas').getContext == "function";
	
	// get number as integer
	var Num = function(i) { return parseInt(i,10) || 0; };
		
	// get lowest number from array
	/*
	var asNum = function(a, b) { return a-b; };
	var getMin = function(a) {
		var b = a.concat();
		return b.sort(asNum)[0];
	};*/
	
	// a basic replacement for jquery .css()
	// getStyle(elm,BorderTopWidth,border-top-width)
	var getStyle = function(el,styleProp,styleProp2) {
		var x = el,y;
		if (x.currentStyle) {
			y = x.currentStyle[styleProp];
		} else if (window.getComputedStyle) {
			if (typeof arguments[2] == "string") styleProp = styleProp2;
			y = document.defaultView.getComputedStyle(x,null).getPropertyValue(styleProp);
		}
		return y;
	};
	
	var getBorderColor = function (elm,p) {
		return getStyle(elm,'border'+p+'Color','border-'+p.toLowerCase()+'-color')
	};

	var getBorderWidth = function(elm,p) {

		if (elm.currentStyle && !isOpera) {
			w = elm.currentStyle['border'+p+'Width'];
			if (w == 'thin') w = 2;
			if (w == 'medium' && !(elm.currentStyle['border'+p+'Style'] == 'none')) w = 4;
			if (w == 'thick') w = 6;
		} else {
			p = p.toLowerCase();
			w = document.defaultView.getComputedStyle(elm,null).getPropertyValue('border-'+p+'-width');
		}
		return Num(w);
	};
	
	var isElm = function(elm,i) {
		return elm.tagName.toLowerCase() == i;
	};
	
	var rotationSteps = function(r_type,a,b,c,d) {
		if (r_type == 'tl') return a;
		if (r_type == 'tr') return b;
		if (r_type == 'bl') return c;
		if (r_type == 'br') return d;
	};
	
	// draw the round corner in Canvas object
	var drawCorner = function(canvas,radius,r_type,bg_color,border_width,border_color,corner_effect) {
		
		var steps,curve_to;
		
		// change rgba(1,2,3,0.9) to rgb(1,2,3)
		if (bg_color.indexOf('rgba') != -1) {
			var reg = /^rgba\((\d{1,3}),\s*(\d{1,3}),\s*(\d{1,3}),\s*(\d{1,3})\)$/;   
			var bits = reg.exec(bg_color);
			if (bits) {
				var channels = [Num(bits[1]),Num(bits[2]),Num(bits[3])];
				bg_color = 'rgb('+channels[0]+', '+channels[1]+', '+channels[2]+')';
			} 
		}
		
		var ctx = canvas.getContext('2d');
		
		if (radius == 1 || corner_effect == 'notch') {
			
			if (border_width > 0 && radius > 1) {
				ctx.fillStyle = border_color;
				ctx.fillRect(0,0,radius,radius);
				ctx.fillStyle = bg_color;
				steps = rotationSteps(r_type,[0-border_width,0-border_width],[border_width,0-border_width],[0-border_width,border_width],[border_width,border_width]);
				ctx.fillRect(steps[0],steps[1],radius,radius);
			} else {
				ctx.fillStyle = bg_color;
				ctx.fillRect(0,0,radius,radius);
			}
			return canvas;
		} else if (corner_effect == 'bevel') {
			steps = rotationSteps(r_type,[0,0,0,radius,radius,0,0,0],[0,0,radius,radius,radius,0,0,0],[0,0,radius,radius,0,radius,0,0],[radius,radius,radius,0,0,radius,radius,radius]);
			ctx.fillStyle = bg_color;
			ctx.beginPath();
			ctx.moveTo(steps[0],steps[1]);
			ctx.lineTo(steps[2], steps[3]);
			ctx.lineTo(steps[4], steps[5]);
			ctx.lineTo(steps[6], steps[7]);
			ctx.fill(); 
			if (border_width > 0 && border_width < radius) {
				ctx.strokeStyle = border_color;
	        	ctx.lineWidth = border_width;
    			ctx.beginPath();
				steps = rotationSteps(r_type,[0,radius,radius,0],[0,0,radius,radius],[radius,radius,0,0],[0,radius,radius,0]);
    			ctx.moveTo(steps[0],steps[1]);
				ctx.lineTo(steps[2],steps[3]);
    			ctx.stroke();
			}
			return canvas;
		}

		steps = rotationSteps(r_type,
					[0,0,radius,0,radius,0,0,radius,0,0],
					[radius,0,radius,radius,radius,0,0,0,0,0],
					[0,radius,radius,radius,0,radius,0,0,0,radius],
					[radius,radius,radius,0,radius,0,0,radius,radius,radius]);
         
		ctx.fillStyle = bg_color;
    	ctx.beginPath();
     	ctx.moveTo(steps[0],steps[1]); 
     	ctx.lineTo(steps[2], steps[3]);
    	if(r_type == 'br') ctx.bezierCurveTo(steps[4], steps[5], radius, radius, steps[6], steps[7]);
    	else ctx.bezierCurveTo(steps[4], steps[5], 0, 0, steps[6], steps[7]);
		ctx.lineTo(steps[8], steps[9]);
        ctx.fill(); 
         
        // draw border
        if (border_width > 0 && border_width < radius) {
	        
	        // offset caused by border
	        var offset = border_width/2; 
	        var ro = radius-offset;
			steps = rotationSteps(r_type,
				[ro,offset,ro,offset,offset,ro],
				[ro,ro,ro,offset,offset,offset],
				[ro,ro,offset,ro,offset,offset,offset,ro],
				[ro,offset,ro,offset,offset,ro,ro,ro]	
			);

			curve_to = rotationSteps(r_type,[0,0],[0,0],[0,0],[radius, radius]);

	        ctx.strokeStyle = border_color;
	        ctx.lineWidth = border_width;
    		ctx.beginPath();
    		// go to corner to begin curve
     		ctx.moveTo(steps[0], steps[1]); 
     		// curve from righttop to leftbottom (for the tl canvas)
    		ctx.bezierCurveTo(steps[2], steps[3], curve_to[0], curve_to[1], steps[4], steps[5]); 
			ctx.stroke();
	        
	    }
	    
	    return canvas;
	    
	};
	
	// create and append canvas element to parent
	var createCanvas = function(p,radius) {
		
		var elm = document.createElement('canvas');
		elm.setAttribute("height", radius);
		elm.setAttribute("width", radius); 
	    elm.style.display = "block";
		elm.style.position = "absolute";
		elm.className = "jrCorner";
		
		appendToParent(p,elm);
		
		if (!canvasSupport && isMSIE) { // no native canvas support
			if (typeof G_vmlCanvasManager == "object") { // use excanvas
				elm = G_vmlCanvasManager.initElement(elm);
			} else if (typeof G_vmlCMjrc == "object") { // use the stipped down version of excanvas
				elm = G_vmlCMjrc.i(elm);
			} else {
				 throw Error('Could not find excanvas');
			}
		}
		
		return elm;
	};
	
	var appendToParent = function(p,elm) {
		if (p.is("table")) {
			p.children("tbody").children("tr:first").children("td:first").append(elm); 
			p.css('display','block'); // only firefox seems to need this
		} else if(p.is("td")) {
			if (p.children(".JrcTdContainer").length === 0) {
				// only is msie you can absolute position a element inside a table cell, so we need a wrapper
				p.html('<div class="JrcTdContainer" style="padding:0px;position:relative;margin:-1px;zoom:1;">'+p.html()+'</div>');
				p.css('zoom','1');
				if (isltMSIE7) { //  msie6 only
					p.children(".JrcTdContainer").get(0).style.setExpression("height","this.parentNode.offsetHeight"); 
				}
				
			} 
			p.children(".JrcTdContainer").append(elm); 
			
		} else {
			p.append(elm); 
		}

	};
	
	// hide corners in ie print
	// (using canvas {display:none} doesnt work)
	if (isMSIE) {
		var ss = document.createStyleSheet(); 
		ss.media = 'print';
    	ss.cssText = '.jrcIECanvasDiv { display:none !important; }';
    }
	
    // $.corner function
	var _corner = function(options) {
		
		// nothing to do || no support for native canvas or excanvas
		if (this.length==0 || !(canvasSupport || isMSIE)) {
			return this;
		}	
		
		if (options == "destroy") {
			return this.each(function() {
				var p, elm = $(this);
				if (elm.is(".jrcRounded")) {
					if (typeof elm.data("ie6tmr.jrc") == 'number') window.clearInterval(elm.data("ie6tmr.jrc"));
					if (elm.is("table")) p = elm.children("tbody").children("tr:first").children("td:first");
					else if (elm.is("td")) p = elm.children(".JrcTdContainer");
					else p = elm;
					p.children(".jrCorner").remove();
					elm.unbind('mouseleave.jrc').unbind('mouseenter.jrc').removeClass('jrcRounded').removeData('ie6tmr.jrc');
					if (elm.is("td")) elm.html(elm.children(".JrcTdContainer").html());
				}
			});
		}
			
		// interpret the (string) argument
   		var o = (options || "").toLowerCase();
   		var radius = Num((o.match(/(\d+)px/)||[])[1]) || "auto"; // corner width
   		var bg_arg = ((o.match(/(#[0-9a-f]+)/)||[])[1]) || "auto";  // strip color
   		var re = /round|bevel|notch/; // Corner Effects
    	var fx = ((o.match(re)||['round'])[0]);
    	var hover = /hover/.test(o);
    	var overSized = /oversized/.test(o);
    	var hiddenparent_arg = o.match("hiddenparent");
    	if (isMSIE) {
    		var re = /ie6nofix|ie6fixinit|ie6fixexpr|ie6fixonload|ie6fixwidthint|ie6fixheightint|ie6fixbothint/; // Type of iefix
    		var ie6Fix = ((o.match(re)||['ie6fixinit'])[0]);
    	} 	
    	
   		//var edges = { T:0, B:1 };
    	var opts = {
        	tl:  /top|left|tl/.test(o),       
        	tr:  /top|right|tr/.test(o),
        	bl:  /bottom|left|bl/.test(o),    
        	br:  /bottom|right|br/.test(o)
    	};
    	
    	// round all corners if nothing is set
    	if ( !opts.tl && !opts.tr && !opts.bl && !opts.br) opts = { tl:1, tr:1, bl:1, br:1 };
    	       	
		this.each(function() {

			var elm = $(this),rbg=null,bg,s,b,pr;
			var a = this;
			var elm_display = getStyle(this,'display');
			var elm_position = getStyle(this,'position');
			var elm_lineheight = getStyle(this,'lineHeight','line-height');
					
			if (bg_arg == "auto") { // no background color of the parent is set ...
				s = elm.siblings(".jrcRounded:eq(0)");
				if (s.length > 0) { // sibling already has the parent background color stored?
					b = s.data("rbg.jrc");
					if (typeof b == "string") {
						rbg = b;
					}
				}
			}
			
			if (hiddenparent_arg || rbg === null) {
				// temporary show hidden parent (wm.morgun) + search for background color
				var current_p = this.parentNode, hidden_parents = new Array(),a = 0;
				while( (typeof current_p == 'object') && !isElm(current_p,'html') ) {
					
					if (hiddenparent_arg && getStyle(current_p,'display') == 'none') {
						hidden_parents.push({
							originalvisibility: getStyle(current_p,'visibility'),
							elm: current_p
						});
						current_p.style.display = 'block';
						current_p.style.visibility = 'hidden';
					}
					var pbg = getStyle(current_p,'backgroundColor','background-color');
					if (rbg === null && pbg != "transparent" && pbg != "rgba(0, 0, 0, 0)") {
						rbg = pbg;
					}
					
					current_p = current_p.parentNode;
	
				}
				
				if (rbg === null) rbg = "#ffffff";
			}
			
			// store the parent background color
			if (bg_arg == "auto") {
				bg = rbg;
				elm.data("rbg.jrc",rbg);
			} else {
				bg = bg_arg;
			}
			
			// if element is hidden we cant get the size..
			if (elm_display == 'none') {
				var originalvisibility = getStyle(this,'visibility');
				this.style.display = 'block';
				this.style.visibility = 'hidden';
				var ishidden = true;
			} else {
				var ishiddden = false;
			}
			
			// save width/height
			var elm_height = elm.height();
			var elm_width = elm.width();
			
			// hover (optional argument - for a alterative to #roundedelement:hover)
			if (hover) {
				
				var newOptions = o.replace(/hover|ie6nofix|ie6fixinit|ie6fixexpr|ie6fixonload|ie6fixwidthint|ie6fixheightint|ie6fixbothint/g, "");
				if (ie6Fix != 'ie6nofix') newOptions = "ie6fixinit "+newOptions;
				
				elm.bind("mouseenter.jrc", function(){
					elm.addClass('jrcHover');
					elm.corner(newOptions);
				});
				elm.bind("mouseleave.jrc", function(){
					elm.removeClass('jrcHover');
					elm.corner(newOptions);
				});
				
			}
			
	   		// msie6 rendering bugs 
			if (isltMSIE7 && ie6Fix != 'ie6nofix') {
				
				this.style.zoom = 1;
				
				//if (this.currentStyle['height'] == 'auto') {
				//	elm.height(elm_height); 
				//}
				
				// http://www.pmob.co.uk/temp/onepxgap.htm
				if (ie6Fix != 'ie6fixexpr') {
					if (elm.width()%2 != 0)elm.width(elm.width()+1);
			 		if (elm.height()%2 != 0) elm.height(elm.height()+1);
			 	}

			 	$(window).load(function () {
					 	if (ie6Fix == 'ie6fixonload') {
							if (elm.css('height') == 'auto') elm.height(elm.css('height')); 
				 			if (elm.width()%2 != 0) elm.width(elm.width()+1);
				 			if (elm.height()%2 != 0) elm.height(elm.height()+1);
			 			} else if (ie6Fix == 'ie6fixwidthint' || ie6Fix == 'ie6fixheightint' || ie6Fix == 'ie6fixbothint') {
				 			var myInterval,ie6FixFunction;
				 			if (ie6Fix == 'ie6fixheightint') {
					 			ie6FixFunction = function () {
									elm.height('auto');
									var elm_height = elm.height();
									if (elm_height%2 != 0) elm_height = elm_height+1;
		  							elm.css({height:elm_height}); 
								};
							} else if (ie6Fix == 'ie6fixwidthint') {
								ie6FixFunction = function () {
									elm.width('auto');
									var elm_width = elm.width();
		  							if (elm_width%2 != 0) elm_width = elm_width+1;
		  							elm.css({width:elm_width}); 
		  							elm.data('lastWidth.jrc',elm.get(0).offsetWidth);
								};
							} else if(ie6Fix == 'ie6fixbothint') {
								ie6FixFunction = function () {
									elm.width('auto');
									elm.height('auto');
									var elm_width = elm.width();
									var elm_height = elm.height();
									if (elm_height%2 != 0) elm_height = elm_height+1;
		  							if (elm_width%2 != 0) elm_width = elm_width+1;
		  							elm.css({width:elm_width,height:elm_height}); 
								};
							} 
							myInterval = window.setInterval(ie6FixFunction,100);
							elm.data("ie6tmr.jrc",myInterval);
				 		}
    			});

			 	// ie6fixwidthint|ie6fixheightint
			 	
			 	//this.style.setExpression("height","parseInt(this.clientHeight) % 2 != 0 ? parseInt(this.style.height)-1 : parseInt(this.style.height)"); 
			 	//alert(this.style.height);
			 	//if (elm_lineheight != 'normal' && elm_height < elm_lineheight) {
				// 	elm.css('lineHeight', elm_height);
				//}
			}
			
			// get lowest height/width
			/*
			var arr = [this.offsetHeight,this.offsetWidth];
			if (elm_height != 0) arr[arr.length] = elm_height;
			if (elm_width != 0) arr[arr.length] = elm_width;
			var widthHeightSmallest = getMin(arr);*/
			
			var widthHeightSmallest = elm_height < elm_width ? this.offsetHeight : this.offsetWidth;
			
			
			// the size of the corner is not defined...
			if (radius == "auto") {
				radius = widthHeightSmallest/2;
				if (radius > 10) radius = widthHeightSmallest/4; 
			}

			// the size of the corner can't be to high
			if (radius > widthHeightSmallest/2 && !overSized) {
				radius = widthHeightSmallest/2;
			}
			
			radius = Math.floor(radius);
			
			// get border width
			var border_t = getBorderWidth(this, 'Top');
			var border_r = getBorderWidth(this, 'Right');
			var border_b = getBorderWidth(this, 'Bottom');
			var border_l = getBorderWidth(this, 'Left');
			
			// some css thats required in order to position the canvas elements
			if (elm_position == 'static' && !isElm(this,'td')) { 
				//elm.css('position','relative'); 
				this.style.position = 'relative';
			// only needed for ie6 and (ie7 in Quirks mode) , CSS1Compat == Strict mode
			} else if (elm_position == 'fixed' && isMSIE && !(document.compatMode == 'CSS1Compat' && !isltMSIE7)) { 
				//elm.css('position','absolute');
				this.style.position = 'absolute';
			}
			
			// overflow hidden + border makes the real borders at the corners visible
			// so we set overflow to visible when it has borders
			if (border_t+border_r+border_b+border_l > 0) {
				//elm.css('overflow','visible');
				this.style.overflow = 'visible';
			}
			
			// restore css
			if (ishidden) elm.css({display:'none',visibility:originalvisibility});
			
			//  restore css of hidden parents
			if (typeof hidden_parents != "undefined") {
				for (var i = 0; i < hidden_parents.length; i++) {
					hidden_parents[i].elm.style.display = 'none';
					hidden_parents[i].elm.style.visibility = hidden_parents[i].originalvisibility;
				}
			}	
			
			var p_top = 0-border_t, 
				p_right = 0-border_r, 
				p_bottom = 0-border_b,
				p_left = 0-border_l;
				
			var mhc = (elm.find("canvas").length > 0);
			
			if (mhc) {
				// pr is the parent of the canvas elements
				if (isElm(this,'table')) pr = elm.children("tbody").children("tr:first").children("td:first");
				else if (isElm(this,'td')) pr = elm.children(".JrcTdContainer");
				else pr = elm;
			}
	
			// draw Corners in canvas elements (createCanvas also appends it to parent)
			if (opts.tl) { 
				// use lowest border size
				bordersWidth = border_t < border_l ? border_t : border_l;
				// remove old corner
				if (mhc) pr.children("canvas.jrcTL").remove(); 
				// create,append and draw corner
				var tl = drawCorner(createCanvas(elm,radius),radius,'tl',bg,bordersWidth,getBorderColor(this,'Top'),fx); 
				// position corner
				$(tl).css({left:p_left,top:p_top}).addClass('jrcTL'); 
			}
			if (opts.tr) { 
				bordersWidth = border_t < border_r ? border_t : border_r;
				if (mhc) pr.children("canvas.jrcTR").remove();
				var tr = drawCorner(createCanvas(elm,radius),radius,'tr',bg,bordersWidth,getBorderColor(this,'Top'),fx); 
				$(tr).css({right:p_right,top:p_top}).addClass('jrcTR'); 
			}
			if (opts.bl) { 
				bordersWidth = border_b < border_l ? border_b : border_l;
				if (mhc) pr.children("canvas.jrcBL").remove();
				var bl = drawCorner(createCanvas(elm,radius),radius,'bl',bg,bordersWidth,getBorderColor(this,'Bottom'),fx);
				$(bl).css({left:p_left,bottom:p_bottom}).addClass('jrcBL');  
			}
			if (opts.br) { 
				bordersWidth = border_b < border_r ? border_b : border_r;
				if (mhc) pr.children("canvas.jrcBR").remove();
				var br = drawCorner(createCanvas(elm,radius),radius,'br',bg,bordersWidth,getBorderColor(this,'Bottom'),fx);
				$(br).css({right:p_right,bottom:p_bottom}).addClass('jrcBR');
			}
			
			// we need this to hide it in ie print
			if (isMSIE) elm.children('canvas.jrCorner').children('div').addClass('jrcIECanvasDiv');
			
			// based on fix: http://www.ilikespam.com/blog/the-odd-pixel-bug
			if (isltMSIE7 && ie6Fix == 'ie6fixexpr') {
				if (opts.bl) {
					bl.style.setExpression("bottom","this.parentNode.offsetHeight % 2 == 0 || this.parentNode.offsetWidth % 2 == 0 ? 0-(parseInt(this.parentNode.currentStyle['borderBottomWidth'])) : 0-(parseInt(this.parentNode.currentStyle['borderBottomWidth'])+1)");
				}
				if (opts.br) {
					br.style.setExpression("right", "this.parentNode.offsetWidth  % 2 == 0 || this.parentNode.offsetWidth % 2 == 0 ? 0-(parseInt(this.parentNode.currentStyle['borderRightWidth']))  : 0-(parseInt(this.parentNode.currentStyle['borderRightWidth'])+1)");
					br.style.setExpression("bottom","this.parentNode.offsetHeight % 2 == 0 || this.parentNode.offsetWidth % 2 == 0 ? 0-(parseInt(this.parentNode.currentStyle['borderBottomWidth'])) : 0-(parseInt(this.parentNode.currentStyle['borderBottomWidth'])+1)");
				}
				if (opts.tr) {
					tr.style.setExpression("right","this.parentNode.offsetWidth   % 2 == 0 || this.parentNode.offsetWidth % 2 == 0 ? 0-(parseInt(this.parentNode.currentStyle['borderRightWidth']))  : 0-(parseInt(this.parentNode.currentStyle['borderRightWidth'])+1)");
				}	
			}
			
			elm.addClass('jrcRounded');
			
				
   		});  
   		
   		// callback function (is called when the last element is rounded)
		if (typeof arguments[1] == "function") arguments[1](this); 
   		return this;
   		
	};
	
	$.fn.corner = _corner;
	
})(jQuery);
