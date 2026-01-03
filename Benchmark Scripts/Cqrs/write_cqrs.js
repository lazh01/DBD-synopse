import http from 'k6/http';
import { sleep } from 'k6';

export const options = {
    vus: 50,
    duration: '30s',
};

let users = [];
let products = [];

export function setup() {
    const usersRes = http.get('http://localhost:5002/api/seed/users');
    const productsRes = http.get('http://localhost:5002/api/seed/products');

    users = usersRes.json();
    products = productsRes.json();

    return { users, products };
}

export default function (data) {
    const { users, products } = data;

    const user = users[Math.floor(Math.random() * users.length)];

    const lines = [];
    const numLines = Math.floor(Math.random() * 3) + 1;
    for (let i = 0; i < numLines; i++) {
        const product = products[Math.floor(Math.random() * products.length)];
        lines.push({ ProductId: product.id, Quantity: Math.floor(Math.random() * 5) + 1 });
    }

    const payload = JSON.stringify({
        UserId: user.id,
        Lines: lines,
    });

    const params = {
        headers: { 'Content-Type': 'application/json' },
    };

    const res = http.post('http://localhost:5002/api/orders', payload, params);
    console.log(`Status: ${res.status}`);

}