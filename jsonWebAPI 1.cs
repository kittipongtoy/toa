/**
 * The `node:vm` module enables compiling and running code within V8 Virtual
 * Machine contexts.
 *
 * **The `node:vm` module is not a security**
 * **mechanism. Do not use it to run untrusted code.**
 *
 * JavaScript code can be compiled and run immediately or
 * compiled, saved, and run later.
 *
 * A common use case is to run the code in a different V8 Context. This means
 * invoked code has a different global object than the invoking code.
 *
 * One can provide the context by `contextifying` an
 * object. The invoked code treats any property in the context like a
 * global variable. Any changes to global variables caused by the invoked
 * code are reflected in the context object.
 *
 * ```js
 * const vm = require('node:vm');
 *
 * const x = 1;
 *
 * const context = { x: 2 };
 * vm.createContext(context); // Contextify the object.
 *
 * const code = 'x += 40; var y = 17;';
 * // `x` and `y` are global variables in the context.
 * // Initially, x has the value 2 because that is the value of context.x.
 * vm.runInCon