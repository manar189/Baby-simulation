function [t, z] = ode2euler(fun, z0, u, N, h)

z = zeros(length(z0), N);
t = (0:h:N*h-h);
z(:,1) = z0;                % initial condidtions

for n = 1:N-1
    z(1,n+1) = z(1,n) + h*z(2,n);    % position
    z(2,n+1) = z(2,n) + h*fun(z(1,n), z(2,n), u(n)); % velocity
end

end

