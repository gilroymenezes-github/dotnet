namespace BlazorRcl.Interops {

    const wait = (ms) => new Promise(res => setTimeout(res, ms));
    const startAsync = async callbackWith => {
        await wait(1000);
        callbackWith('Hello!');
        await wait(1000);
        callbackWith('And welcome..');
        await wait(1000);
        callbackWith('To async await in Typescript!');
    }

    class Hello {
        public showPrompt(message: string): string {
            return prompt(message, 'Typescript anything here');
        }

        public async greetMe() {
            return startAsync(text => console.log(text));
        }
        
    }

    export function Load(): void {
        window['hello'] = new Hello();
    }
}

BlazorRcl.Interops.Load();