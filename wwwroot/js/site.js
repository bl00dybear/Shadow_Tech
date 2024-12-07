document.addEventListener("DOMContentLoaded", () => {
    // Selectăm toate butoanele de tip "Adaugă în coș"
    const addToCartButtons = document.querySelectorAll('.add-to-cart');

    addToCartButtons.forEach(button => {
        button.addEventListener('click', (event) => {
            // Prevenim comportamentul implicit al linkului
            event.preventDefault();

            // Extragem informațiile din atributele `data-*`
            const productId = button.getAttribute('data-product-id');
            const productName = button.getAttribute('data-product-name');
            const productPrice = button.getAttribute('data-product-price');

            // Creăm obiectul produsului
            const product = {
                id: productId,
                name: productName,
                price: parseFloat(productPrice)
            };

            // Preluăm coșul din sessionStorage sau creăm unul nou
            let cart = JSON.parse(sessionStorage.getItem('cart')) || [];

            // Adăugăm produsul în coș
            cart.push(product);

            // Salvăm coșul în sessionStorage
            sessionStorage.setItem('cart', JSON.stringify(cart));

            // Opțional: notificăm utilizatorul
            alert(`${productName} a fost adăugat în coș!`);
        });
    });
});
