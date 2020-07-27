var BlazorRcl;
(function (BlazorRcl) {
    var Interops;
    (function (Interops) {
        const wait = (ms) => new Promise(res => setTimeout(res, ms));
        const startAsync = async (callback) => {
            await wait(1000);
            callback('Hello!');
            await wait(1000);
            callback('And welcome..');
            await wait(1000);
            callback('To async await in Typescript!');
        };
        class Hello {
            showPrompt(message) {
                return prompt(message, 'Typescript anything here');
            }
            async greetMe() {
                return startAsync(text => console.log(text));
            }
        }
        function Load() {
            window['hello'] = new Hello();
        }
        Interops.Load = Load;
    })(Interops = BlazorRcl.Interops || (BlazorRcl.Interops = {}));
})(BlazorRcl || (BlazorRcl = {}));
BlazorRcl.Interops.Load();
//# sourceMappingURL=Hello.js.map