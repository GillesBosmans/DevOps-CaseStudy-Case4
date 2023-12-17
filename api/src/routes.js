const express = require('express');
const router = express.Router();
const Login = require('./models/login');

const app = express();
const bodyParser = require('body-parser');
app.use(bodyParser.json());

/********************/
/*      Routes      */
/********************/

// get all logins
// router.get('/', async (req, res) => {
//     console.log('/get all logins called');
//     try {
//         res.json(await Login.find());
//     } catch (err) {
//         console.log(err),
//             res.sendStatus(500);
//     }
// });


// get login by username and password
router.post('/login', async (req, res) => {
    console.log('/login route called');
    try {
        const {
            username,
            password
        } = req.body;

        // Check if the username and password match a user in the sample data
        const user = await Login.findOne({
            username,
            password
        });

        if (user) {
            // Attach the token and user role to the response
            res.json({
                message: 'Login successful',
                name: username,
                role: user.role
            });
        } else {
            res.status(401).json({
                message: 'Invalid credentials',
                role: null
            });
        }
    } catch (err) {
        console.log(err);
        res.status(500).json(err);
    }
});


module.exports = router;