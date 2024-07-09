import { check } from 'k6';
import http from 'k6/http';

/**
 * Test configuration
 */
export const options = {
    tags: {
        test: 'public-api',
        test_run_id: 'e-Shop-On-Web-Load',
    },
    scenarios: {
        // Load testing using K6 constant-rate scenario
        // getCatalogItems_scenario: {
        //     executor: 'constant-arrival-rate',
        //     rate: 100000, // number of iterations per time unit
        //     timeUnit: '1m', // iterations will be per minute
        //     duration: '10m', // total duration that the test will run for
        //     preAllocatedVUs: 50, // the size of the VU (i.e. worker) pool for this scenario
        //     maxVUs: 500, // if the preAllocatedVUs are not enough, we can initialize more
        //     tags: { test_type: 'getCatalogItems' }, // different extra metric tags for this scenario
        //     exec: 'getCatalogItems',// Test scenario function to call
        // },
        getAllCatalogItems_scenario: {
            executor: 'constant-arrival-rate',
            rate: 100, // number of iterations per time unit
            timeUnit: '1m', // iterations will be per minute
            duration: '2m', // total duration that the test will run for
            preAllocatedVUs: 10, // the size of the VU (i.e. worker) pool for this scenario
            maxVUs: 50, // if the preAllocatedVUs are not enough, we can initialize more
            tags: { test_type: 'getAllCatalogItems' }, // different extra metric tags for this scenario
            exec: 'getAllCatalogItems',// Test scenario function to call
        }
    }
};

/**
 * prepare the test data like authentication
 * @returns Initial data for each test case
 */
export function setup() {
    const apiEndpoint = 'https://e-shop-on-web-api.azurewebsites.net/api/';
    return { endpoint: apiEndpoint };
}

/**
 * Get Catalog Items test case
 */
// export function getCatalogItems(data) {

//     const url = `${data.endpoint}catalog-items`;
//     const headers = {
//         'accept': 'application/json',
//     };

//     const payload = null;
//     var response = http.get(url, payload, { headers: headers });

//     check(response, { 'status is 200': (r) => r.status === 200 });
//     if (response.status != 200) {
//         console.log(`operation: getCatalogItems, url: ${url}, Status:${response.status}`);
//     }
// }

/**
 * Get All Catalog Items test case
 */
export function getAllCatalogItems(data) {

    const pageSize = getRandomInt(12);
    const pageIndex = getRandomInt(Math.floor(12/pageSize));
    const url = `${data.endpoint}catalog-items?pageSize=${pageSize}&pageIndex=${pageIndex}`;
    const headers = {
        'accept': 'application/json',
    };

    const payload = null;
    var response = http.get(url, payload, { headers: headers });

    check(response, { 'status is 200': (r) => r.status === 200 });
    if (response.status != 200) {
        console.log(`operation: getAllCatalogItems, url: ${url}, Status:${response.status}`);
    }
}

function getRandomInt(max) {
    return Math.floor(Math.random() * max);
}