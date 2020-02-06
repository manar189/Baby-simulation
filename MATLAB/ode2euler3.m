function [t, z] = ode2euler3(fun, z0, u, N, h)

z = zeros(length(z0), N);
t = (0:h:N*h-h);
z(:,1) = z0;        % initial condidtions

for n = 1:N-1
    z(1,n+1) = z(1,n) + h*z(2,n);   % x position
    z(3,n+1) = z(3,n) + h*z(4,n);   % y position
    [ax, ay] = fun(z(1,n), z(3,n), z(2,n), z(4,n), u(:,n)); % accelerations
    z(2,n+1) = z(2,n) + h*ax;       % x velocity
    z(4,n+1) = z(4,n) + h*ay;       % y velocity
end

end

