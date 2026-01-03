import http from 'k6/http';
import { sleep } from 'k6';

export const options = {
    vus: 50,
    duration: '30s',
};

export default function () {

    let res = http.get('http://localhost:5002/api/orders');

    console.log(`Status: ${res.status}`);

}