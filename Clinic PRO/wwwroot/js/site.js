(function () {
    // ظل للشريط العلوي عند النزول
    const nav = document.querySelector('.clinic-nav');
    const onScroll = () => nav && nav.classList.toggle('scrolled', window.scrollY > 8);
    onScroll();
    window.addEventListener('scroll', onScroll, { passive: true });

    // ظهور العناصر تدريجيا عند السكرول
    const items = document.querySelectorAll('.reveal');
    if ('IntersectionObserver' in window) {
        const io = new IntersectionObserver((entries) => {
            entries.forEach((e) => {
                if (e.isIntersecting) {
                    e.target.classList.add('visible');
                    io.unobserve(e.target);
                }
            });
        }, { threshold: 0.15 });
        items.forEach((el) => io.observe(el));
    } else {
        items.forEach((el) => el.classList.add('visible'));
    }

    // اختفاء رسائل النجاح/الخطأ تلقائيا بعد 6 ثواني
    document.querySelectorAll('.alert').forEach((a) => {
        setTimeout(() => {
            a.style.transition = 'opacity .6s ease, transform .6s ease';
            a.style.opacity = '0';
            a.style.transform = 'translateY(-8px)';
            setTimeout(() => a.remove(), 600);
        }, 6000);
    });
})();