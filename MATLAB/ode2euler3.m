function [t, z] = ode2euler3(fun1, fun2, z0, u, N, h)

z = zeros(length(z0), N);
t = (0:h:N*h-h);
z(:,1) = z0;               % initial condidtions

for n = 1:N-1
    z(1,n+1) = z(1,n) + h*z(2,n);    % x position
    z(3,n+1) = z(3,n) + h*z(4,n);    % y position
    z(2,n+1) = z(2,n) + h*fun1(z(1,n), z(3,n), z(2,n), u(:,n)); % x velocity
    z(4,n+1) = z(4,n) + h*fun2(z(1,n), z(3,n), z(4,n), u(:,n)); % y velocity
end

end

