
function wait() {
    setTimeout(() => { console.log(new Date().toLocaleDateString()); }, 1000);
    return "Time is: ";
}

class Hello {

    public showPrompt(message: string): any {
        return prompt(message, 'Typescript anything here');
    }

    public async greetMe() {

        const result =  wait();
        console.log(result);
    }

}

export function Main(): void {
   
    (window as any).hello = new Hello();
}

Main();