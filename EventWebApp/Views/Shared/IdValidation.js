//Function to validate Estonian ID code
function isValidEstonianId(id) {
    if (!/^\d{11}$/.test(id)) return false;

    // Weight arrays for checksum calculation
    const firstWeights = [1, 2, 3, 4, 5, 6, 7, 8, 9, 1];
    const secondWeights = [3, 4, 5, 6, 7, 8, 9, 1, 2, 3];

    // Calculate checksums
    const calculateChecksum = (weights) =>
        id
            .substring(0, 10)
            .split('')
            .reduce((sum, current, index) => sum + parseInt(current, 10) * weights[index], 0) % 11;

    let checksum = calculateChecksum(firstWeights);

    // If first checksum calculation equals 10, recalculate using second weight array
    if (checksum === 10) {
        checksum = calculateChecksum(secondWeights);
        // If second calculation also equals 10, checksum is 0
        checksum = checksum === 10 ? 0 : checksum;
    }

    // Compare calculated checksum to the last digit of the ID
    return checksum === parseInt(id[10], 10);
}