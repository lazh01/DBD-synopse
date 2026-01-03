import http from 'k6/http';
import { check } from 'k6';
import { sleep } from 'k6';
export const options = {
    vus: 50,
    duration: '30s',
};

let users = [];
let products = [];

export function setup() {
    const usersRes = http.get('http://localhost:5003/api/seed/users');
    const productsRes = http.get('http://localhost:5003/api/seed/products');

    users = usersRes.json();
    products = productsRes.json();

    for (let i = 0; i < 10; i++) {
        const user = users[Math.floor(Math.random() * users.length)];

        const lines = [];
        const numLines = Math.floor(Math.random() * 3) + 1;
        for (let j = 0; j < numLines; j++) {
            const product = products[Math.floor(Math.random() * products.length)];
            lines.push({
                ProductId: product.id,
                Quantity: Math.floor(Math.random() * 5) + 1
            });
        }

        const order = {
            UserId: user.id,
            Lines: lines
        };

        const res = http.post('http://localhost:5003/api/orders', JSON.stringify(order), {
            headers: { 'Content-Type': 'application/json' },
        });

        check(res, { 'order created': (r) => r.status === 200 });
    }

    console.log('Setup complete, 10 orders created.');
    sleep(10);
    return { users, products };
}

export default function (data) {
    const res = http.get('http://localhost:5003/api/orders');
    console.log(`Status: ${res.status}`);
}