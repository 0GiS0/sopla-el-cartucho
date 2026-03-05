var SoplaElCartucho = SoplaElCartucho || {};

$(document).ready(function() {
    console.log('🎮 ¡Sopla el Cartucho iniciado!');
    console.log('💨 Framework: jQuery ' + $.fn.jquery);
    
    SoplaElCartucho.init();
});

SoplaElCartucho = {
    init: function() {
        this.initCarrito();
        this.initAnimaciones();
        this.initFormularios();
        this.initEasterEggs();
    },
    
    initCarrito: function() {
        var self = this;
        
        setInterval(function() {
            self.actualizarBadgeCarrito();
        }, 30000);
    },
    
    actualizarBadgeCarrito: function() {
        $.ajax({
            url: '/Carrito/ObtenerCantidad',
            type: 'GET',
            dataType: 'json',
            success: function(data) {
                if (data && data.cantidad !== undefined) {
                    var badge = $('.carrito-badge');
                    if (data.cantidad > 0) {
                        if (badge.length === 0) {
                            $('a[href*="Carrito"]').append('<span class="carrito-badge">' + data.cantidad + '</span>');
                        } else {
                            badge.text(data.cantidad);
                        }
                    } else {
                        badge.remove();
                    }
                }
            },
            error: function() {
                console.log('Error actualizando carrito');
            }
        });
    },
    
    initAnimaciones: function() {
        setInterval(function() {
            $('.blink').each(function() {
                $(this).css('opacity', Math.random() > 0.1 ? 1 : 0.7);
            });
        }, 150);
        
        $('.typing-effect').each(function() {
            var text = $(this).text();
            var $el = $(this);
            $el.text('');
            var i = 0;
            
            var interval = setInterval(function() {
                if (i < text.length) {
                    $el.text($el.text() + text.charAt(i));
                    i++;
                } else {
                    clearInterval(interval);
                }
            }, 50);
        });
    },
    
    initFormularios: function() {
        $('form').on('submit', function() {
            var $form = $(this);
            var $btn = $form.find('button[type="submit"]');
            
            if ($form.data('submitting')) {
                return false;
            }
            
            $form.data('submitting', true);
            $btn.prop('disabled', true).text('CARGANDO...');
            
            setTimeout(function() {
                $form.data('submitting', false);
                $btn.prop('disabled', false);
            }, 5000);
        });
        
        $('input[required]').on('blur', function() {
            var $input = $(this);
            if (!$input.val()) {
                $input.css('border-color', '#ff0000');
            } else {
                $input.css('border-color', '#333');
            }
        });
    },
    
    initEasterEggs: function() {
        var konamiCode = [38, 38, 40, 40, 37, 39, 37, 39, 66, 65];
        var konamiIndex = 0;
        
        $(document).on('keydown', function(e) {
            if (e.keyCode === konamiCode[konamiIndex]) {
                konamiIndex++;
                if (konamiIndex === konamiCode.length) {
                    SoplaElCartucho.activarModoRetro();
                    konamiIndex = 0;
                }
            } else {
                konamiIndex = 0;
            }
        });
        
        var clickCount = 0;
        $('.header h1').on('click', function() {
            clickCount++;
            if (clickCount >= 5) {
                alert('🎮 ¡Has encontrado un easter egg!\n\n¡Gracias por explorar Sopla el Cartucho!');
                clickCount = 0;
            }
        });
    },
    
    activarModoRetro: function() {
        console.log('🎮 ¡CÓDIGO KONAMI ACTIVADO! 🎮');
        
        $('body').css({
            'background-image': 'repeating-linear-gradient(0deg, rgba(0,255,0,0.03), rgba(0,255,0,0.03) 1px, transparent 1px, transparent 2px)',
            'animation': 'crt-flicker 0.15s infinite'
        });
        
        var style = $('<style>@keyframes crt-flicker { 0% { opacity: 0.97; } 50% { opacity: 1; } 100% { opacity: 0.98; } }</style>');
        $('head').append(style);
        
        console.log('%c' + 
            '   _____ ____  _____  _               \n' +
            '  / ____/ __ \\|  __ \\| |        /\\    \n' +
            ' | (___| |  | | |__) | |       /  \\   \n' +
            '  \\___ \\ |  | |  ___/| |      / /\\ \\  \n' +
            '  ____) | |__| | |    | |____ / ____ \\ \n' +
            ' |_____/ \\____/|_|    |______/_/    \\_\\\n' +
            '  EL CARTUCHO 💨                      \n',
            'color: #00ff00; font-family: monospace;'
        );
        
        alert('🎮 ¡MODO RETRO ACTIVADO!\n\n↑ ↑ ↓ ↓ ← → ← → B A\n\n¡Has desbloqueado el modo nostálgico!');
    },
    
    formatearPrecio: function(precio) {
        return precio.toFixed(2).replace('.', ',') + ' €';
    },
    
    mostrarNotificacion: function(mensaje, tipo) {
        tipo = tipo || 'success';
        var bgColor = tipo === 'success' ? '#002200' : '#220000';
        var borderColor = tipo === 'success' ? '#00ff00' : '#ff0000';
        
        var $toast = $('<div>')
            .text(mensaje)
            .css({
                'position': 'fixed',
                'bottom': '20px',
                'right': '20px',
                'background': bgColor,
                'border': '2px solid ' + borderColor,
                'color': borderColor,
                'padding': '15px 25px',
                'font-family': "'Press Start 2P', monospace",
                'font-size': '10px',
                'z-index': '10000',
                'opacity': '0'
            })
            .appendTo('body')
            .animate({ opacity: 1 }, 200);
        
        setTimeout(function() {
            $toast.animate({ opacity: 0 }, 200, function() {
                $(this).remove();
            });
        }, 3000);
    }
};

function confirmarAccion(mensaje) {
    return confirm(mensaje);
}

function irAPagina(url) {
    window.location.href = url;
}

if (!window.console) {
    window.console = {
        log: function() {},
        warn: function() {},
        error: function() {}
    };
}
