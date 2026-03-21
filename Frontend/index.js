// Use https (http secure).
// http (non-secure) will cause mixed content errors when running from Azure.
const localUrl = "http://localhost:5217/events"
const azureUrl = "https://your-azure-app.azurewebsites.net/events" // update after deploy

const localAuthUrl = "http://localhost:5217/auth/login"
const azureAuthUrl = "https://your-azure-app.azurewebsites.net/auth/login"

Vue.createApp({
    data() {
        return {
            isLocal: true,
            token: null,
            role: null,
            loginData: { username: "", password: "" },
            loginMessage: "",
            events: [],
            eventsMessage: "",
            categoryFilter: "",
            deleteId: null,
            deleteMessage: "",
            addData: { title: "", date: "", category: "", price: null },
            addMessage: ""
        }
    },
    computed: {
        baseUrl() {
            return this.isLocal ? localUrl : azureUrl
        },
        authUrl() {
            return this.isLocal ? localAuthUrl : azureAuthUrl
        },
        authHeaders() {
            return this.token
                ? { Authorization: "Bearer " + this.token }
                : {}
        }
    },
    async created() {
        this.getAll()
    },
    methods: {
        toggleServer() {
            this.isLocal = !this.isLocal
            this.getAll()
        },
        async login() {
            this.loginMessage = ""
            try {
                const response = await axios.post(this.authUrl, this.loginData)
                this.token = response.data.token
                this.role  = response.data.role
                this.getAll()
            } catch (ex) {
                this.loginMessage = ex.message
            }
        },
        logout() {
            this.token = null
            this.role  = null
        },
        async getAll() {
            this.getEvents(this.baseUrl)
        },
        async getByCategory(category) {
            if (!category) {
                alert("Please enter a category to filter by")
                return
            }
            const url = this.baseUrl + "?category=" + category
            this.getEvents(url)
        },
        sortBy(attribute) {
            const url = this.baseUrl + "?sortBy=" + attribute
            this.getEvents(url)
        },
        sortByDesc(attribute) {
            const url = this.baseUrl + "?sortBy=" + attribute + "&descending=true"
            this.getEvents(url)
        },
        async getEvents(url) { // helper method used by getAll, getByCategory, sortBy
            try {
                const response = await axios.get(url)
                this.events = response.data
                this.eventsMessage = "Loaded " + this.events.length + " events"
            } catch (ex) {
                alert(ex.message)
            }
        },
        async addEvent() {
            if (!this.addData.title || !this.addData.category ||
                this.addData.price === null || this.addData.price <= 0) {
                alert("Please fill in all fields with valid values")
                return
            }
            try {
                const response = await axios.post(this.baseUrl, this.addData, {
                    headers: this.authHeaders
                })
                this.addMessage = "Response " + response.status + " " + response.statusText
                this.getAll()
            } catch (ex) {
                alert(ex.message)
            }
        },
        async deleteEvent(id) {
            if (id === null || id === undefined || isNaN(id) || id <= 0) {
                alert("Please enter a valid event ID")
                return
            }
            const url = this.baseUrl + "/" + id
            try {
                const response = await axios.delete(url, {
                    headers: this.authHeaders
                })
                this.deleteMessage = "Response " + response.status + " " + response.statusText
                this.getAll()
            } catch (ex) {
                alert(ex.message)
            }
        }
    }
}).mount("#app")